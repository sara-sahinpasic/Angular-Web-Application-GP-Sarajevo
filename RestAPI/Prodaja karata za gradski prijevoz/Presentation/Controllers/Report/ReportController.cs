using Application.Config;
using Application.DataClasses.Report.Ticket;
using Application.Services.Abstractions.Interfaces.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Controllers.Report;

[ApiController]
[Route("[controller]/[action]")]
public sealed class ReportController : ControllerBase
{
	private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Policy = AuthorizationPolicies.AdminUserPolicyName)]
    [HttpGet]
    public async Task<IActionResult> DownloadPurchaseHistoryReport(Guid userId, CancellationToken cancellationToken)
    {
        byte[] pdfContents = await _reportService.GetPurchaseHistoryReportAsync(userId, cancellationToken: cancellationToken);

        return File(pdfContents, "application/octet-stream", $"{userId} purchase history.pdf");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpGet("/Admin/[controller]/Daily")]
    public async Task<IActionResult> DailyReport()
    {
        DateTime date = DateTime.Now;

        TicketReportData ticketReportData = await _reportService.GetTicketReportForDateAsync(date);

        Response response = new()
        {
            Data = ticketReportData
        };

        return Ok(response);
    }
}
