using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Admin.Vehicles;

namespace Presentation.Controllers.Admin.Vehicles
{
    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [ApiController]
    [Route("[controller]")]
    public sealed class AdminVehicleController : ControllerBase
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IObjectMapperService _mapperService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminVehicleController(IManufacturerRepository manufacturerRepository, IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            _manufacturerRepository = manufacturerRepository;
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _mapperService = mapperService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("/Manufacturers")]
        public async Task<IActionResult> GetAllManufacturers(CancellationToken cancellationToken)
        {
            var data = await _manufacturerRepository.GetAll()
                .ToArrayAsync(cancellationToken);

            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }

        [HttpGet("/VehicleType")]
        public async Task<IActionResult> GetAllVehiclesType(CancellationToken cancellationToken)
        {
            var data = await _vehicleTypeRepository.GetAll()
                .ToArrayAsync(cancellationToken);

            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }

        [HttpPost("/Vehicle")]
        public async Task<IActionResult> CreateVehicle(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            if (await _vehicleRepository.IsVehicleRegisteredAsync(vehicleDto.RegistrationNumber, cancellationToken))
            {
                Response errorResponse = new()
                {
                    Message = "Vozilo već postoji u bazi podataka.",
                    Data = vehicleDto.RegistrationNumber,
                };

                return BadRequest(errorResponse);
            }

            Vehicle newVehicle = new();
            _mapperService.Map(vehicleDto, newVehicle);

            await _vehicleRepository.CreateAsync(newVehicle, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Novo vozilo registrovano.",
            };
            
            return CreatedAtAction(nameof(CreateVehicle), response);
        }

        [HttpPost("/VehicleType")]
        public async Task<IActionResult> CreateVehicleType(VehicleTypeDto vehicleTypeDto, CancellationToken cancellationToken)
        {
            if (await _vehicleTypeRepository.IsVehicleTypeRegisteredAsync(vehicleTypeDto.Name, cancellationToken))
            {
                Response errorResponse = new()
                {
                    Message = "Tip vozila već postoji u bazi podataka.",
                    Data = vehicleTypeDto.Name
                };

                return BadRequest(errorResponse);
            }

            VehicleType newVehicleType = new();
            _mapperService.Map(vehicleTypeDto, newVehicleType);

            await _vehicleTypeRepository.CreateAsync(newVehicleType, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Novi tip vozila registrovan.",
            };
            
            return CreatedAtAction(nameof(CreateVehicleType), response);
        }
        [HttpPost("/Manufacturer")]
        public async Task<IActionResult> CreateManufacturer(ManufacturerDto manufacturerDto, CancellationToken cancellationToken)
        {
            if (await _manufacturerRepository.IsManufacturerRegisteredAsync(manufacturerDto.Name, cancellationToken))
            {
                Response errorResponse = new()
                {
                    Message = "Proizvođač vozila već postoji u bazi podataka.",
                    Data = manufacturerDto.Name
                };
                return BadRequest(errorResponse);
            }
            
            Manufacturer newManufacturer = new();
            
            _mapperService.Map(manufacturerDto, newManufacturer);
            await _manufacturerRepository.CreateAsync(newManufacturer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);
            
            Response response = new()
            {
                Message = "Novi proizvođač vozila registrovan.",
            };
            return CreatedAtAction(nameof(CreateManufacturer), response);
        }
    }
}
