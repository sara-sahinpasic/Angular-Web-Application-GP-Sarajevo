using Application.Config;
using Application.Services.Abstractions.Interfaces.Driver;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Driver;
using Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Driver;

namespace Presentation.Controllers.Driver;

[Authorize(Policy = AuthorizationPolicies.DriverPolicyName)]
[ApiController]
[Route("Driver/[controller]")]
public class MalfunctionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapperService _mapperService;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMalfunctionRepository _malfunctionRepository;
    private readonly IMalfunctionService _malfunctionService;

    public MalfunctionController(IUnitOfWork unitOfWork,
        IObjectMapperService mapperService, IVehicleRepository vehicleRepository, 
        IMalfunctionRepository malfunctionRepository, IMalfunctionService malfunctionService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _vehicleRepository = vehicleRepository;
        _malfunctionRepository = malfunctionRepository;
        _malfunctionService = malfunctionService;
    }

    [HttpPost("Notify")]
    public async Task<IActionResult> AddMalfunction(MalfunctionDto malfunctionDto, CancellationToken cancellationToken)
    {
        Malfunction newMalfunction = new();
        _mapperService.Map(malfunctionDto, newMalfunction);

        if (!malfunctionDto.Fixed)
        {
            Vehicle vehicle = await _vehicleRepository.GetByIdEnsuredAsync(newMalfunction.VehicleId, cancellationToken: cancellationToken);
            vehicle.HasMalfunction = true; 
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
        }

        await _malfunctionRepository.CreateAsync(newMalfunction, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        if (!malfunctionDto.Fixed)
        {
            await _malfunctionService.SendMalfunctionNotification(newMalfunction, cancellationToken);
        }

        Response response = new()
        {
            Message = "Kvar zabilježen.",
        };
        
        return CreatedAtAction(nameof(AddMalfunction), response);
    }
}