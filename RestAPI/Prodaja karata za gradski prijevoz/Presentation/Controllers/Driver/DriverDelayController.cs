using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Admin.Vehicles;
using Presentation.DTO.Driver;
using Presentation.DTO.Routes;

namespace Presentation.Controllers.Driver
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DriverDelayController : ControllerBase
    {
        private readonly IRouteRepository routeRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IObjectMapperService mapperService;
        private readonly IDelayRepository delayRepository;

        public DriverDelayController(IRouteRepository routeRepository, IUnitOfWork unitOfWork,
            IObjectMapperService mapperService, IDelayRepository delayRepository)
        {
            this.routeRepository = routeRepository;
            this.unitOfWork = unitOfWork;
            this.mapperService = mapperService;
            this.delayRepository = delayRepository;
        }

        [HttpGet("/Routes")]
        public async Task<IActionResult> GetAllRoutess(CancellationToken cancellationToken)
        {
            var data = await routeRepository.GetAll()
                .Select(route => new RouteResponseDto
                {
                    Id = route.Id,
                    StartingLocation = route.StartStation.Name,
                    EndingLocation = route.EndStation.Name,
                })
                .ToArrayAsync(cancellationToken);

            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }

        [HttpPost("/Delay")]
        public async Task<IActionResult> AddDelay(DelayDto delayDto, CancellationToken cancellationToken)
        {
            Domain.Entities.Driver.Delay newDelay = new();
            mapperService.Map(delayDto, newDelay);

            delayRepository.Create(newDelay);
            await unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Dodano kašnjenje.",
            };
            return CreatedAtAction(nameof(AddDelay), response);
        }
    }
}
