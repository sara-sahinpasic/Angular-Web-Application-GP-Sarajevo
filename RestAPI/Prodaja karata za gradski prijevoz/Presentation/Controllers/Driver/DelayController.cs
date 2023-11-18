using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Driver;

namespace Presentation.Controllers.Driver;

[Authorize(Policy = AuthorizationPolicies.DriverPolicyName)]
[ApiController]
[Route("Driver/[controller]")]
public class DelayController : ControllerBase
{
    private readonly IRouteRepository _routeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IObjectMapperService _mapperService;
    private readonly IDelayRepository _delayRepository;

    public DelayController(IRouteRepository routeRepository, IUnitOfWork unitOfWork,
        IObjectMapperService mapperService, IDelayRepository delayRepository)
    {
        _routeRepository = routeRepository;
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _delayRepository = delayRepository;
    }

    [HttpPost("Notify")]
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