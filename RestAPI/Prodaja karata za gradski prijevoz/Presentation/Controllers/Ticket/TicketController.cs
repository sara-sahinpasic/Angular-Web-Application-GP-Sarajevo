using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketEntity = Domain.Entities.Tickets.Ticket;
using Presentation.DTO;
using Presentation.DTO.Admin.Ticket;
using Microsoft.AspNetCore.Authorization;
using Application.Config;

namespace Presentation.Controllers.Ticket;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("[controller]")]
public sealed class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TicketController(ITicketRepository ticketRepository, IUnitOfWork unitOfWork)
    {
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
    }

    [AllowAnonymous]
    [HttpGet("All")]
    public async Task<IActionResult> GetAllCardTypes(bool includeInactive, CancellationToken cancellationToken)
    {
        TicketEntity[] tickets = await _ticketRepository.GetAll(includeInactive)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = tickets
        };

        return Ok(response);
        // todo: we will need a repository for card type creation and read which means we will need to remove the enum
    }

    [HttpPut("/Admin/[controller]/Edit/{id}")]
    public async Task<IActionResult> EditTicket([FromServices] IObjectMapperService objectMapper, Guid id, 
        TicketDto ticketDto, CancellationToken cancellationToken)
    {
        Response response = new();

        TicketEntity? ticket = await _ticketRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (ticket is null)
        {
            response.Message = "Karta nije pronađena";
            
            return NotFound(response);
        }

        objectMapper.Map(ticketDto, ticket);

        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno ste editovali kartu.";

        return Ok(response);
    }

    [HttpDelete("/Admin/[controller]/Delete/{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id, CancellationToken cancellationToken)
    {
        Response response = new();
        TicketEntity? ticket = await _ticketRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (ticket is null)
        {
            response.Message = "Karta nije pronađena";

            return NotFound(response);
        }

        await _ticketRepository.DeleteAsync(ticket, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno obrisana karta";

        return Ok(response);
    }
}
