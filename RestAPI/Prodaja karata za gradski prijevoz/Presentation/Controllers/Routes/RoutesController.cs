using Application.Config;
using Application.DataClasses;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Domain.Entities.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Routes;
using Presentation.Validators.Routes;
using System.Linq.Expressions;

namespace Presentation.Controllers.Routes;

[ApiController]
[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[Route("[controller]")]
public sealed class RoutesController : ControllerBase
{
	private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoutesController(IRouteRepository routeRepository, IUnitOfWork unitOfWork)
    {
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
    }

    [AllowAnonymous]
    [HttpGet("GetByDate")]
    public async Task<IActionResult> GetAllRoutesForDateAction(Guid startStationId, Guid endStationId, DateTime date, CancellationToken cancellationToken)
    {
        DateTime currentDate = DateTime.Now;

        if (date < currentDate)
        {
            date = currentDate;
        }

        Expression<Func<Route, SelectedRouteResponse>> selector = (route) => new SelectedRouteResponse
        {
            Id = route.Id,
            StartingLocation = route.StartStation.Name,
            EndingLocation = route.EndStation.Name,
            Time = route.TimeOfDeparture.ToString("c"),
            Date = date
        };

        ICollection<SelectedRouteResponse> routesList = await _routeRepository.GetRoutesByDateAsync(startStationId, endStationId, date, selector, cancellationToken);

        Response response = new()
        {
            Data = routesList
        };

        if (routesList.Count == 0)
        {
            response.Message = "route_controller_no_routes_found_message";

            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllRoutes(CancellationToken cancellationToken)
    {
        IReadOnlyList<RoutesListResponseDto> routesList = await _routeRepository.GetAll()
            .Select(route => new RoutesListResponseDto
            {
                Id = route.Id,
                Active = route.Active,
                ActiveOnHolidays = route.ActiveOnHolidays,
                ActiveOnWeekends = route.ActiveOnWeekends,
                EndingLocation = route.EndStation.Name,
                StartingLocation = route.StartStation.Name,
                TimeOfArrival = route.TimeOfArrival.ToString("c"),
                TimeOfDeparture = route.TimeOfDeparture.ToString("c"),
                VehicleNumber = route.Vehicle.Number
            })
            .ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = routesList
        };

        return Ok(response);
    }

    [HttpGet("/Get/{routeId}")]
    public async Task<IActionResult> GetRoute([FromServices] IObjectMapperService mapperService, Guid routeId, CancellationToken cancellationToken)
    {
        Route route = await _routeRepository.GetByIdEnsuredAsync(routeId, cancellationToken: cancellationToken);

        EditRouteResponseDto routeResponseData = new();

        mapperService.Map(route, routeResponseData);

        routeResponseData.TimeOfDeparture = route.TimeOfDeparture.ToString("c");
        routeResponseData.TimeOfArrival = route.TimeOfArrival.ToString("c");

        Response response = new()
        {
            Data = routeResponseData
        };

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateRoutes(List<CreateRouteRequestDto> routesToCreateRequest, CancellationToken cancellationToken)
    {
        ValidatorResult validatorResult = new();

        foreach (CreateRouteRequestDto routeRequest in routesToCreateRequest)
        {
            validatorResult = RouteValidator.ValidateRouteRequest(routeRequest);
            
            if (!validatorResult.IsValid)
            {
                Response errorResponse = new()
                {
                    Message = validatorResult.ErrorMessage
                };

                return BadRequest(errorResponse);
            }
        }

        IReadOnlyList<Route> routes = routesToCreateRequest.Select(route => new Route
        {
            StartStationId = route.StartStationId,
            EndStationId = route.EndStationId,
            TimeOfDeparture = TimeSpan.Parse(route.TimeOfDeparture),
            TimeOfArrival = TimeSpan.Parse(route.TimeOfArrival),
            Active = route.Active,
            ActiveOnHolidays = route.ActiveOnHolidays,
            ActiveOnWeekends = route.ActiveOnWeekends,
            VehicleId = route.VehicleId
        })
        .ToList();

        await _routeRepository.CreateRangeAsync(routes, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno kreirane rute."
        };
        
        return CreatedAtAction(nameof(CreateRoutes), response);
    }

    [HttpPut("Edit/{routeId}")]
    public async Task<IActionResult> EditRoute([FromServices] IObjectMapperService mapperService, Guid routeId, 
        EditRouteRequestDto routeRequest, CancellationToken cancellationToken)
    {
        ValidatorResult validatorResult = RouteValidator.ValidateRouteRequest(routeRequest);

        if (!validatorResult.IsValid)
        {
            Response errorResponse = new()
            {
                Message = validatorResult.ErrorMessage
            };

            return BadRequest(errorResponse);
        }

        Route route = await _routeRepository.GetByIdEnsuredAsync(routeId, cancellationToken: cancellationToken);
        
        mapperService.Map(routeRequest, route);

        route.TimeOfDeparture = TimeSpan.Parse(routeRequest.TimeOfDeparture);
        route.TimeOfArrival = TimeSpan.Parse(routeRequest.TimeOfArrival);

        await _routeRepository.UpdateAsync(route, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno editovana ruta."
        };
        
        return Ok(response);
    }

    [HttpPut("Deactivate/{routeId}")]
    public async Task<IActionResult> DeactivateRoute(Guid routeId, CancellationToken cancellationToken)
    {
        Route route = await _routeRepository.GetByIdEnsuredAsync(routeId, cancellationToken: cancellationToken);

        route.Active = false;

        await _routeRepository.UpdateAsync(route, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno deaktivirana ruta."
        };
        
        return Ok(response);
    }

}
