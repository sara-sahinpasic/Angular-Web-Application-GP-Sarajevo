using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Domain.Entities.Requests;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Presentation.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public sealed class RequestContoller : ControllerBase
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public RequestContoller(IRequestRepository requestRepository,
            IHostingEnvironment hostingEnvironment, IUnitOfWork unitOfWork)
        {
            this._requestRepository = requestRepository;
            this._hostingEnvironment = hostingEnvironment;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile()
        {
            if (!Request.Form.Files.Any())
                return BadRequest("No files found in the request");

            if (Request.Form.Files.Count > 1)
                return BadRequest("Cannot upload more than one file at a time");

            if (Request.Form.Files[0].Length <= 0)
                return BadRequest("Invalid file length, seems to be empty");

            try
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                string uploadsDir = Path.Combine(webRootPath, "uploads");

                // wwwroot/uploads/
                if (!Directory.Exists(uploadsDir))
                    Directory.CreateDirectory(uploadsDir);

                IFormFile file = Request.Form.Files[0];
                string fileName = Path.GetRandomFileName() + "." + file.ContentType.Split("/")[1];
                string fullPath = Path.Combine(uploadsDir, fileName);

                var buffer = 1024 * 1024;
                using var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, buffer, useAsync: false);
                await file.CopyToAsync(stream);
                await stream.FlushAsync();

                string location = $"images/{fileName}";

                var result = new
                {
                    message = "Upload successful",
                    url = location
                };
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var a = JsonSerializer.Deserialize<Request>(Request.Form["requestData"], options);
                Request newRequest = new Request()
                {
                    Active= true,
                    DateCreated=DateTime.Now,
                    UserStatusId=a.UserStatusId,
                    DocumentLink=location
                };
                _requestRepository.Create(newRequest);
                await _unitOfWork.CommitAsync(default);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Upload failed: " + ex.Message);
            }
        }
    }
}
