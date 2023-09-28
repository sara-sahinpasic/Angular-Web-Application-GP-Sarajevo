using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Domain.Entities.Routes;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Routes;
using System.Linq.Expressions;

namespace Presentation.Controllers.Routes;

[ApiController]
[Route("[controller]")]
public sealed class RoutesController : ControllerBase
{
	private readonly IRouteRepository _routeRepository;

    public RoutesController(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    [HttpGet("GetByDate")]
    public async Task<IActionResult> GetAllRoutesAction(Guid startStationId, Guid endStationId, DateTime date, CancellationToken cancellationToken)
    {
        DateTime currentDate = DateTime.Now;

        if (date < currentDate)
        {
            date = currentDate;
        }

        Expression<Func<Route, RouteResponseDto>> selector = (route) => new RouteResponseDto
        {
            Id = route.Id,
            StartingLocation = route.StartStation.Name,
            EndingLocation = route.EndStation.Name,
            Time = route.TimeOfDeparture.ToString("c"),
            Date = date
        };

        IEnumerable<RouteResponseDto> routesList = await _routeRepository.GetRoutesByDateAsync(startStationId, endStationId, date, selector, cancellationToken);

        Response response = new()
        {
            Data = routesList
        };

        if (routesList.Count() == 0)
        {
            response.Message = "route_controller_no_routes_found_message";
            return NotFound(response);
        }

        return Ok(response);
    }
}
