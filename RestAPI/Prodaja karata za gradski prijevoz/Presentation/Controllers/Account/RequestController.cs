
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

        [HttpPost("UploadFile")]
        [RequestSizeLimit(RequestSizeLimit)]
        public async Task<IActionResult> UploadFileAction([FromForm] SpecialRequestRequestDto specialRequestRequestDto, CancellationToken cancellationToken)
        {
            bool hasAnyActiveRequests = await _requestRepository.HasAnyActiveRequests(specialRequestRequestDto.UserId, cancellationToken);

            if (hasAnyActiveRequests)
            {
                Response<string?> activeRequestErrorResponse = new()
                {
                    Message = "One request has been sent already. Please, wait until it's processed."
                };

                return BadRequest(activeRequestErrorResponse);
            }

            string[] acceptedFileExtensions = { "jpg", "jpeg", "png", "pdf" };
            IFormFile file = specialRequestRequestDto.Document;

            string? filePath = await _fileService.UploadFileAsync(acceptedFileExtensions, file, cancellationToken);

            if (filePath is null)
            {
                Response<string?> errorResponse = new()
                {
                    Message = "File extension not valid"
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

            Response<string?> response = new()
            {
                Message = "Successfully sent the request",
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
