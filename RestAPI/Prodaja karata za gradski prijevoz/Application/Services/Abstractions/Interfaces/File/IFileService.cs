using Microsoft.AspNetCore.Http;

namespace Application.Services.Abstractions.Interfaces.File;

public interface IFileService
{
    Task<string?> UploadFileAsync(string[] acceptedExtensions, IFormFile file, CancellationToken cancellationToken = default);
}