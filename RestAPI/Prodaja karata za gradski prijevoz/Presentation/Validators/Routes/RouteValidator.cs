

using Application.DataClasses;
using Presentation.DTO.Routes;

namespace Presentation.Validators.Routes;

public static class RouteValidator
{
    public static ValidatorResult ValidateRouteRequest(CreateRouteRequestDto routeRequest)
    {
        ValidatorResult validatorResult = new()
        {
            IsValid = true
        };
            
        if (routeRequest.StartStationId == routeRequest.EndStationId) 
        {
            validatorResult.IsValid = false;
            validatorResult.ErrorMessage = "Startna stanica ne može biti ista kao završna stanica.";

            return validatorResult;
        }

        return validatorResult;
    }
}
