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
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public sealed class AdminVehicleContoller : ControllerBase
    {
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IVehicleTypeRepository vehicleTypeRepository;
        private readonly IObjectMapperService mapperService;
        private readonly IUnitOfWork unitOfWork;

        public AdminVehicleContoller(IManufacturerRepository manufacturerRepository, IVehicleRepository vehicleRepository,
            IVehicleTypeRepository vehicleTypeRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            this.manufacturerRepository = manufacturerRepository;
            this.vehicleRepository = vehicleRepository;
            this.vehicleTypeRepository = vehicleTypeRepository;
            this.mapperService = mapperService;
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("/Manufacturers")]
        public async Task<IActionResult> GetAllManufacturers(CancellationToken cancellationToken)
        {
            var data = await manufacturerRepository.GetAll()
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
            var data = await vehicleTypeRepository.GetAll()
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
            if (await vehicleRepository.IsVehicleRegisteredAsync(vehicleDto.RegistrationNumber))
            {
                Response errorResponse = new()
                {
                    Message = "Vozilo već postoji u bazi podataka.",
                    Data = vehicleDto.RegistrationNumber,
                };

                return BadRequest(errorResponse);
            }

            Vehicle newVehicle = new();
            mapperService.Map(vehicleDto, newVehicle);

            vehicleRepository.Create(newVehicle);
            await unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Novo vozilo registrovano.",
            };
            return CreatedAtAction(nameof(CreateVehicle), response);
        }
    }
}
