using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

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

    //todo: make error handling better
    // todo: create response object to store data and such (maybe generic request as well?)
    public IActionResult ErrorHandler() 
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var exceptionMessage = exceptionFeature.Error.InnerException?.Message ?? exceptionFeature.Error.Message;

        // todo: see how to do with logger while creating better exception handling
        System.IO.File.AppendAllText("Logs/log_exceptions.log", $"{DateTime.Now}: {exceptionMessage}\n");

        ProblemDetails problem = new()
        {
            Title = "Error",
            Detail = "Something went wrong. Contact site administrator.",
            Status = 500
        };

        return StatusCode(500, problem);
    }
}
