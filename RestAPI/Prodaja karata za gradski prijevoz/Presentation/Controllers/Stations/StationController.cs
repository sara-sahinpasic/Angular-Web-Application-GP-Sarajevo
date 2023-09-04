using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Application.Services.Abstractions.Interfaces.Repositories.Stations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Stations;

namespace Presentation.Controllers.Stations;

[ApiController]
[Route("[controller]")]
public sealed class StationController : ControllerBase
{
    private readonly IStationRepository _stationRepository;
    private readonly IRouteRepository _routeRepository;

    public StationController(IStationRepository stationRepository, IRouteRepository routeRepository)
    {
        _stationRepository = stationRepository;
        _routeRepository = routeRepository;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAction(CancellationToken cancellationToken)
    {
        IEnumerable<StationResponseDto> stations = await _stationRepository.GetAll()
            .AsNoTracking()
            .Select(station => new StationResponseDto
            {
                Id = station.Id,
                Name = station.Name,
            })
            .ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = stations
        };

        return Ok(response);
    }

    [HttpGet("GetRouted")]
    public async Task<IActionResult> GetRoutedAction(Guid startStationId, CancellationToken cancellationToken)
    {
        IEnumerable<StationResponseDto> stations = await _routeRepository.GetAll()
            .AsNoTracking()
            .Where(route => route.StartStationId == startStationId)
            .Select(route => new StationResponseDto
            {
                Id = route.EndStationId,
                Name = route.EndStation.Name,
            })
            .Distinct()
            .ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = stations
        };

        return Ok(response);
    }
}
