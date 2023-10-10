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

namespace Presentation.Controllers.Driver
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DriverMalfunctionController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IObjectMapperService mapperService;
        private readonly IVehicleRepository vehicleRepository;
        private readonly IMalfunctionRepository malfunctionRepository;
        private readonly IManufacturerRepository manufacturerRepository;

        public DriverMalfunctionController(IUnitOfWork unitOfWork,
            IObjectMapperService mapperService, IVehicleRepository vehicleRepository, IMalfunctionRepository malfunctionRepository)
        {
            this.unitOfWork = unitOfWork;
            this.mapperService = mapperService;
            this.vehicleRepository = vehicleRepository;
            this.malfunctionRepository = malfunctionRepository;
        }

        [HttpGet("/Vehicles")]
        public async Task<IActionResult> GetAllVehicles(CancellationToken cancellationToken)
        {
            var data = await vehicleRepository.GetAll()
                .Select(vehicle => new VehicleDto
                {
                    Id = vehicle.Id,
                    RegistrationNumber = vehicle.RegistrationNumber
                })
                .ToArrayAsync(cancellationToken);

            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }

        [HttpPost("/Malfunction")]
        public async Task<IActionResult> AddMalfunction(MalfunctionDto malfunctionDto, CancellationToken cancellationToken)
        {
            Malfunction newMalfunction = new();
            mapperService.Map(malfunctionDto, newMalfunction);

            malfunctionRepository.Create(newMalfunction);
            await unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Kvar zabilježen.",
            };
            return CreatedAtAction(nameof(AddMalfunction), response);
        }
    }
}
