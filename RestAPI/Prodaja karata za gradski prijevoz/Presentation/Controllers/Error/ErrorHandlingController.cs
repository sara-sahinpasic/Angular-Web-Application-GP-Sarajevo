using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers.Error;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("/error")]
public sealed class ErrorHandlingController : ControllerBase
{
    public ILogger<ErrorHandlingController> _logger { get; }

    public ErrorHandlingController(ILogger<ErrorHandlingController> logger)
    {
        _logger = logger;
    }

    public IActionResult ErrorHandler()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        return HandleException(exceptionFeature.Error);
    }

    private IActionResult HandleException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception, nameof(exception));

        return Problem(exception);
    }

    private IActionResult Problem(Exception exception)
    {
        ProblemDetails problem = new()
        {
            Title = "Error",
            Detail = "Something went wrong. Contact site administrator.",
            Status = 500
        };
        var exceptionMessage = exception.InnerException?.Message ?? exception.Message;
        // todo: see how to do with logger: sprint 3
        //  System.IO.File.AppendAllText("Logs/log_exceptions.log", $"{DateTime.Now}: {exceptionMessage}\n");
        _logger.LogError(exceptionMessage);

        return StatusCode((int)problem.Status, problem);
    }
}
