using Application.Services.Abstractions.Interfaces.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.File;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public sealed class FileController : ControllerBase
{
	private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet]
    public async Task<IActionResult> DownloadPurchaseHistory(Guid userId, CancellationToken cancellationToken)
    {
        byte[] pdfContents = await _fileService.GeneratePurchaseHistoryPDFAsync(userId, cancellationToken);

        return File(pdfContents, "application/octet-stream", $"{userId} purchase history.pdf");
    }
}
