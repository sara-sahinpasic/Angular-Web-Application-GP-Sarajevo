using Application.Services.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Presentation.DTO;

namespace Prodaja_karata_za_gradski_prijevoz.Filters;

public sealed class LocalizationAttribute : IAsyncActionFilter
{
    private readonly ILocalizationService _localizationService;

    public LocalizationAttribute(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ctx = await next();

        if (ctx.Result is null)
        {
            return;
        }

        string? locale = ctx.HttpContext.Request.Headers["Locale"];

        ObjectResult? result = ctx.Result as ObjectResult;

        if (result?.Value is null || result?.Value.GetType() != typeof(Response)) {
            return;
        } 

        Response? response = result.Value as Response;

        response!.Message = _localizationService.Localize(locale, response.Message);

        result.Value = response;
    }
}
