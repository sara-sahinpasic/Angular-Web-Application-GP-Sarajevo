using Application.Config;
using Application.DataClasses.Report.Route;
using Application.DataClasses.Report.Ticket;
using Application.Services.Abstractions.Interfaces.Report;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;

namespace Presentation.Controllers.Report;

[ApiController]
[Route("[controller]")]
public sealed class ReportController : ControllerBase
{
	private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Policy = AuthorizationPolicies.AdminUserPolicyName)]
    [HttpGet("PurchaseHistory/{userId}")]
    public async Task<IActionResult> DownloadPurchaseHistoryReport(Guid userId, CancellationToken cancellationToken)
    {
        byte[] pdfContents = await _reportService.GetPurchaseHistoryReportAsync(userId, cancellationToken: cancellationToken);

        return File(pdfContents, "application/octet-stream", $"{userId} purchase history.pdf");
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpGet("/Admin/[controller]/Daily")]
    public async Task<IActionResult> DailyReport(CancellationToken cancellationToken)
    {
        DateTime date = DateTime.Now;

        TicketReportData ticketReportData = await _reportService.GetTicketReportForDateAsync(date, cancellationToken);

        Response response = new()
        {
            Data = ticketReportData
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpGet("/Admin/[controller]/Period")]
    public async Task<IActionResult> ReportForPeriod(int month, int year, CancellationToken cancellationToken)
    {
        TicketReportData ticketReportData = await _reportService.GetTicketReportForPeriodAsync(month, year, cancellationToken);

        Response response = new()
        {
            Data = ticketReportData
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpGet("/Admin/[controller]/MostSoldRoute/Period")]
    public async Task<IActionResult> RouteReportForPeriod(int month, int year, CancellationToken cancellationToken)
    {
        RouteReportData? routeReportData = await _reportService.GetRouteWithMostPurchasedTicketsForPeriodAsync(month, year, cancellationToken);

        Response response = new()
        {
            Data = routeReportData
        };

        return Ok(response);
    }
}
