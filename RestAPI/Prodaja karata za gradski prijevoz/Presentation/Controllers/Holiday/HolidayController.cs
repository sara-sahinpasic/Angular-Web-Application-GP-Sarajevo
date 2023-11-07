using Application.Config;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Domain.Entities.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Holiday;
using HolidayEntity = Domain.Entities.Routes.Holiday;

namespace Presentation.Controllers.Holiday;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("[controller]")]
public sealed class HolidayController : ControllerBase
{
    private readonly IHolidayRepository _holidayRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HolidayController(IHolidayRepository holidayRepository, IUnitOfWork unitOfWork)
    {
        _holidayRepository = holidayRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("Get/All")]
    public async Task<IActionResult> GetHolidayList(CancellationToken cancellationToken)
    {
        IReadOnlyCollection<HolidayResponseDto> holidayData = await _holidayRepository.GetAll()
            .Select(holiday => new HolidayResponseDto
            {
                Id = holiday.Id,
                Name = holiday.Name,
                Date = holiday.Date,
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        Response response = new()
        {
            Data = holidayData
        };

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateHoliday([FromServices] IObjectMapperService mapperService, HolidayRequestDto holidayRequest, CancellationToken cancellationToken)
    {
        HolidayEntity holiday = new();

        mapperService.Map(holidayRequest, holiday);

        await _holidayRepository.CreateAsync(holiday, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno ste dodali praznik."
        };

        return CreatedAtAction(nameof(CreateHoliday), response);
    }

    [HttpPut("Edit/{holidayId}")]
    public async Task<IActionResult> EditHoliday([FromServices] IObjectMapperService mapperService, Guid holidayId, 
        HolidayRequestDto holidayRequest, CancellationToken cancellationToken)
    {
        HolidayEntity holiday = await _holidayRepository.GetByIdEnsuredAsync(holidayId, cancellationToken: cancellationToken);

        mapperService.Map(holidayRequest, holiday);

        await _holidayRepository.UpdateAsync(holiday, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno ste editovali praznik."
        };

        return Ok(response);
    }

    [HttpDelete("Delete/{holidayId}")]
    public async Task<IActionResult> DeleteHoliday(Guid holidayId, CancellationToken cancellationToken)
    {
        HolidayEntity holiday = await _holidayRepository.GetByIdEnsuredAsync(holidayId, cancellationToken: cancellationToken);

        await _holidayRepository.DeleteAsync(holiday, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Uspješno ste obrisali praznik"
        };

        return Ok(response);
    }
}
