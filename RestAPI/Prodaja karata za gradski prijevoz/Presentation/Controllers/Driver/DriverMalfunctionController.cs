using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Driver;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Admin.Vehicles;
using Presentation.DTO.Driver;

namespace Presentation.Controllers.Driver;

[Authorize]
[ApiController]
[Route("[controller]")]
public class DriverMalfunctionController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapperService _mapperService;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMalfunctionRepository _malfunctionRepository;

    public DriverMalfunctionController(IUnitOfWork unitOfWork,
        IObjectMapperService mapperService, IVehicleRepository vehicleRepository, IMalfunctionRepository malfunctionRepository)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _vehicleRepository = vehicleRepository;
        _malfunctionRepository = malfunctionRepository;
    }

    [HttpGet("/Vehicles")]
    public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAll()
            .Select(vehicle => new VehicleDto
            {
                Id = vehicle.Id,
                RegistrationNumber = vehicle.RegistrationNumber
            })
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = vehicles
        };

        return Ok(response);
    }

    [HttpPost("/Malfunction")]
    public async Task<IActionResult> AddMalfunction(MalfunctionDto malfunctionDto, CancellationToken cancellationToken)
    {
        Malfunction newMalfunction = new();
        _mapperService.Map(malfunctionDto, newMalfunction);

        await _malfunctionRepository.CreateAsync(newMalfunction, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Kvar zabilježen.",
        };
        
        return CreatedAtAction(nameof(AddMalfunction), response);
    }
}