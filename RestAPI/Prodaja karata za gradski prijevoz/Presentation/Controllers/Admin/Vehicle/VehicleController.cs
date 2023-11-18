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
    [Route("[controller]s")]
    public sealed class VehicleController : ControllerBase
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleTypeRepository _vehicleTypeRepository;
        private readonly IObjectMapperService _mapperService;
        private readonly IUnitOfWork _unitOfWork;

        public VehicleController(IManufacturerRepository manufacturerRepository, IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            _manufacturerRepository = manufacturerRepository;
            _vehicleRepository = vehicleRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
            _mapperService = mapperService;
            _unitOfWork = unitOfWork;
        }

        #region Vehicles

        [HttpPost("Add")]
        public async Task<IActionResult> CreateVehicle(VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            if (await _vehicleRepository.IsVehicleRegisteredAsync(vehicleDto.RegistrationNumber))
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

        [AllowAnonymous]
        [HttpGet("All")]
        public async Task<IActionResult> GetVehicleList(CancellationToken cancellationToken)
        {
            IEnumerable<VehicleListDto> vehicleList = await _vehicleRepository.GetAll()
                .Select(vehicle => new VehicleListDto
                {
                    Id = vehicle.Id,
                    BuildYear = vehicle.BuildYear,
                    Color = vehicle.Color,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = vehicle.Manufacturer.Id,
                        Name = vehicle.Manufacturer.Name,
                    },
                    Number = vehicle.Number,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    Type = new VehicleTypeDto
                    {
                        Id = vehicle.VehicleType.Id,
                        Name = vehicle.VehicleType.Name,
                    }
                })
                .ToListAsync(cancellationToken);

            Response response = new()
            {
                Data = vehicleList
            };

            return Ok(response);
        }

        [HttpDelete("Delete/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(Guid vehicleId, CancellationToken cancellationToken)
        {
            Response response = new();
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken: cancellationToken);

            if (vehicle is null)
            {
                response.Message = "Vozilo ne postoji.";
                return NotFound(response);
            }

            await _vehicleRepository.DeleteAsync(vehicle, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            response.Message = "Uspješno obrisano vozilo";

            return Ok(response);
        }

        [HttpPut("Edit/{vehicleId}")]
        public async Task<IActionResult> EditVehicle(Guid vehicleId, VehicleDto vehicleDto, CancellationToken cancellationToken)
        {
            Response response = new();
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(vehicleId, cancellationToken: cancellationToken);

            if (vehicle is null)
            {
                response.Message = "Nije pronađeno vozilo.";
                return NotFound(response);
            }

            _mapperService.Map(vehicleDto, vehicle);

            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            response.Message = "Uspješno ažurirano vozilo.";
            return Ok(response);
        }

        #endregion

        #region Manufacturers

        [HttpGet("Manufacturers/All")]
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

        [HttpGet("Manufacturers/HasDependentVehicles/{manufacturerId}")]
        public async Task<IActionResult> HasManufacturerDependantVehicles(Guid manufacturerId, CancellationToken cancellationToken)
        {
            bool doesVehicleWithManufacturerExist = await _vehicleRepository.GetAll()
                .AnyAsync(vehicle => vehicle.ManufacturerId == manufacturerId, cancellationToken);

            Response response = new()
            {
                Data = doesVehicleWithManufacturerExist
            };

            return Ok(response);
        }

        [HttpDelete("Manufacturers/Delete/{manufacturerId}")]
        public async Task<IActionResult> DeleteManufacturer(Guid manufacturerId, CancellationToken cancellationToken)
        {
            Response response = new()
            {
                Message = "Uspješno obrisan tip vozila."
            };

            Manufacturer? manufacturer = await _manufacturerRepository.GetByIdAsync(manufacturerId, cancellationToken: cancellationToken);

            if (manufacturer is null)
            {
                response.Message = "Tip vozila nije pronađen.";

                return NotFound(response);
            }

            await _manufacturerRepository.DeleteAsync(manufacturer, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Ok(response);
        }

        [HttpPost("Manufacturers/Add")]
        public async Task<IActionResult> CreateManufacturer(ManufacturerDto manufacturerDto, CancellationToken cancellationToken)
        {
            if (await _manufacturerRepository.IsManufacturerRegisteredAsync(manufacturerDto.Name))
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

        #endregion

        #region Types

        [HttpGet("Types/All")]
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

        [HttpPost("Types/Add")]
        public async Task<IActionResult> CreateVehicleType(VehicleTypeDto vehicleTypeDto, CancellationToken cancellationToken)
        {
            if (await _vehicleTypeRepository.IsVehicleTypeRegisteredAsync(vehicleTypeDto.Name))
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

        [HttpDelete("Types/Delete/{vehicleTypeId}")]
        public async Task<IActionResult> DeleteVehicleType(Guid vehicleTypeId, CancellationToken cancellationToken)
        {
            Response response = new()
            {
                Message = "Uspješno obrisan tip vozila."
            };

            VehicleType? vehicleType = await _vehicleTypeRepository.GetByIdAsync(vehicleTypeId, cancellationToken: cancellationToken);

            if (vehicleType is null)
            {
                response.Message = "Tip vozila nije pronađen.";

                return NotFound(response);
            }

            await _vehicleTypeRepository.DeleteAsync(vehicleType, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Ok(response);
        }

        [HttpGet("Types/HasDependentVehicles/{vehicleTypeId}")]
        public async Task<IActionResult> HasTypeDependantVehicles(Guid vehicleTypeId, CancellationToken cancellationToken)
        {
            bool doesVehicleWithTypeExist = await _vehicleRepository.GetAll()
                .AnyAsync(vehicle => vehicle.VehicleTypeId == vehicleTypeId, cancellationToken);

            Response response = new()
            {
                Data = doesVehicleWithTypeExist
            };

            return Ok(response);
        }
        
        #endregion
    }
}
