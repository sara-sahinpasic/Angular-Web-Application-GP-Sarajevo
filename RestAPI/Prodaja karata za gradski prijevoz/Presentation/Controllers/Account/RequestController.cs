using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Domain.Entities.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Request;

namespace Presentation.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class RequestController : ControllerBase
    {
        private const int RequestSizeLimit = 10 * 1024 * 1024;
        
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestRepository _requestRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RequestController(IFileService fileService, IUnitOfWork unitOfWork, IRequestRepository requestRepository, IWebHostEnvironment hostingEnvironment)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _requestRepository = requestRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("SendRequest")]
        [RequestSizeLimit(RequestSizeLimit)]
        public async Task<IActionResult> SendRequestAction([FromForm] SpecialRequestRequestDto specialRequestRequestDto, CancellationToken cancellationToken)
        {
            bool hasAnyActiveRequests = await _requestRepository.HasAnyActiveRequests(specialRequestRequestDto.UserId, cancellationToken);

            if (hasAnyActiveRequests)
            {
                Response activeRequestErrorResponse = new()
                {
                    Message = "request_controller_send_request_action_multiple_requests_error"
                };

                return BadRequest(activeRequestErrorResponse);
            }

            string[] acceptedFileExtensions = { "jpg", "jpeg", "png", "pdf" };
            IFormFile file = specialRequestRequestDto.Document;

            string? filePath = await _fileService.SaveFileAsync(acceptedFileExtensions, file, cancellationToken: cancellationToken);

            if (filePath is null)
            {
                Response errorResponse = new()
                {
                    Message = "request_controller_send_request_action_file_extension_error"
                };

                return BadRequest(errorResponse);
            }

            Request newRequest = new()
            {
                Active = true,
                DateCreated = DateTime.Now,
                UserStatusId = specialRequestRequestDto.UserStatusId,
                DocumentLink = filePath,
                UserId = specialRequestRequestDto.UserId
            };

            _requestRepository.Create(newRequest);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "request_controller_send_request_action_success",
            };
            
            return Ok(response);
        }

        [HttpGet("UserImage")]
        public async Task<IActionResult> GetImage([FromRoute] string fileName)
        {
            string path = _hostingEnvironment.ContentRootPath + "\\ProfileImages\\";
            var filePath = path + fileName;
            if (System.IO.File.Exists(filePath))
            {
                byte[] b = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(b, "image/*");
            }
            return null;
        }
    }
}
