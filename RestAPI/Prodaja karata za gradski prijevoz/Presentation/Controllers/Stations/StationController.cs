using Application.Config;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Stations;
using Domain.Entities.Stations;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IUnitOfWork _unitOfWork;

    public StationController(IStationRepository stationRepository, IUnitOfWork unitOfWork)
    {
        _stationRepository = stationRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAction(CancellationToken cancellationToken)
    {
        IEnumerable<StationResponseDto> stations = await _stationRepository.GetAll()
            .OrderByDescending(station => station.DateCreated)
            .Select(station => new StationResponseDto
            {
                Id = station.Id,
                Name = station.Name,
            })
            .AsNoTracking()
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
        IReadOnlyCollection<Station> stations = await _stationRepository.GetRoutedStationsAsync(startStationId, cancellationToken);

        IReadOnlyCollection<StationResponseDto> stationResponseData = stations
            .Select(station => new StationResponseDto
            {
                Id = station.Id,
                Name = station.Name
            })
            .ToList();

        Response response = new()
        {
            Data = stationResponseData
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateStation(StationRequestDto request, CancellationToken cancellationToken)
    {
        Station station = new()
        {
            Name = request.Name,
            DateCreated = DateTime.Now
        };

        await _stationRepository.CreateAsync(station, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno kreirana stanica.",
            Data = station
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpPut("Edit/{stationId}")]
    public async Task<IActionResult> EditStation(Guid stationId, StationRequestDto request, CancellationToken cancellationToken)
    {
        Station station = await _stationRepository.GetByIdEnsuredAsync(stationId, cancellationToken: cancellationToken);
        
        station.Name = request.Name;

        await _stationRepository.UpdateAsync(station, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno editovana stanica."
        };

        return Ok(response);
    }

    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [HttpDelete("Delete/{stationId}")]
    public async Task<IActionResult> DeleteStation(Guid stationId, CancellationToken cancellationToken)
    {
        bool isStationPartOfAnyRoute = await _stationRepository.IsStationPartOfAnyRoute(stationId, cancellationToken);

        if (isStationPartOfAnyRoute)
        {
            Response errorResponse = new()
            {
                Message = "Ova stanica je dodijeljena određenim rutama. Molimo Vas, prvo obrišite rute koje sadrže ovu stanicu, potom obrišite stanicu."
            };

            return BadRequest(errorResponse);
        }

        Station station = await _stationRepository.GetByIdEnsuredAsync(stationId, cancellationToken: cancellationToken);

        await _stationRepository.DeleteAsync(station, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno obrisana stanica."
        };

        return Ok(response);
    }
}
