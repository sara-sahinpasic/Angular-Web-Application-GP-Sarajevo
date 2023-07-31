using Domain.Entities.Users;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Abstractions.Interfaces.File;

public interface IFileService
{
    Task<string?> UploadFileAsync(string[] acceptedExtensions, IFormFile file, string folderName = "", CancellationToken cancellationToken = default);

    Task<byte[]> GeneratePurchaseHistoryPDFAsync(Guid userId, CancellationToken cancellationToken = default);
}