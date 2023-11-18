using Application.Services.Abstractions.Interfaces.System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.DTO;
using System.Net;

namespace Presentation.Controllers.Error;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("/error")]
public sealed class ErrorHandlingController : ControllerBase
{
    private ILogger<ErrorHandlingController> _logger { get; }
    private readonly ILogService _logService;

    public ErrorHandlingController(ILogger<ErrorHandlingController> logger, ILogService logService)
    {
        _logger = logger;
        _logService = logService;
    }

    public async Task<IActionResult> ErrorHandlerAsync()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>()!;

        return await HandleException(exceptionFeature.Error);
    }

    private async Task<IActionResult> HandleException(Exception? exception)
    {
        ArgumentNullException.ThrowIfNull(exception, nameof(exception));

        Response response = new()
        {
            Message = "error_controller_general_error"
        };

        var exceptionMessage = exception.InnerException?.Message ?? exception.Message;

        _logger.LogError(exceptionMessage);
        await _logService.LogAsync(exceptionMessage, LogLevel.Error);

        return StatusCode((int)HttpStatusCode.InternalServerError, response);
    }
}
