using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Driver;
using Presentation.DTO.Routes;

namespace Presentation.Controllers.Driver;

[Authorize]
[ApiController]
[Route("[controller]")]
public class DriverDelayController : ControllerBase
{
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapperService _mapperService;
    private readonly IDelayRepository _delayRepository;

    public DriverDelayController(IRouteRepository routeRepository, IUnitOfWork unitOfWork,
        IObjectMapperService mapperService, IDelayRepository delayRepository)
    {
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _delayRepository = delayRepository;
    }

    [HttpGet("/Routes")]
    public async Task<IActionResult> GetAllRoutes(CancellationToken cancellationToken)
    {
        var routesResponseData = await _routeRepository.GetAll()
            .Select(route => new SelectedRouteResponse
            {
                Id = route.Id,
                StartingLocation = route.StartStation.Name,
                EndingLocation = route.EndStation.Name,
            })
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = routesResponseData
        };

        return Ok(response);
    }

    [HttpPost("/Delay")]
    public async Task<IActionResult> AddDelay(DelayDto delayDto, CancellationToken cancellationToken)
    {
        Domain.Entities.Driver.Delay newDelay = new();
        _mapperService.Map(delayDto, newDelay);

        await _delayRepository.CreateAsync(newDelay, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Dodano kašnjenje.",
        };
        
        return CreatedAtAction(nameof(AddDelay), response);
    }
}