using Application.Services.Abstractions.Interfaces.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.File;

public sealed class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public FileService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task<string?> UploadFileAsync(string[] acceptedExtensions, IFormFile file, CancellationToken cancellationToken = default)
    {
        if (!IsValidExtension(acceptedExtensions, file))
        {
            return null;
        }

        string uploadsDir = _hostingEnvironment.ContentRootPath;

        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        string fileName = GetRandomFileName(file);
        string fullPath = Path.Combine(uploadsDir, fileName);

        await using FileStream stream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(stream, cancellationToken);
        await stream.FlushAsync(cancellationToken);

        return fullPath;
    }

    private static string GetRandomFileName(IFormFile file)
    {
        string fileName = Path.GetRandomFileName() + "." + file.ContentType.Split("/")[1];
        return fileName;
    }

    private static bool IsValidExtension(string[] validExtensions, IFormFile file)
    {
        string extension = file.ContentType.Split("/")[1];
        return validExtensions.Contains(extension);
    }
}