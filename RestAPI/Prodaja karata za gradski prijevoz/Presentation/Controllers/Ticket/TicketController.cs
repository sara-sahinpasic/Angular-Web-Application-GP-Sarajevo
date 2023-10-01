using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Admin.Ticket;

namespace Presentation.Controllers.CardType;

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

    [HttpGet("All")]
    public async Task<IActionResult> GetAllCardTypes(bool includeInactive, CancellationToken cancellationToken)
    {
        IQueryable<Ticket> ticketQuery = _ticketRepository.GetAll();

        if (!includeInactive)
        {
            ticketQuery = ticketQuery.Where(ticket => ticket.Active);
        }

        Ticket[] tickets = await ticketQuery
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

        Ticket? ticket = await _ticketRepository.GetByIdAsync(id, cancellationToken);

        if (ticket is null)
        {
            response.Message = "Karta nije pronađena";
            
            return NotFound(response);
        }

        objectMapper.Map(ticketDto, ticket);

        _ticketRepository.Update(ticket);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno ste editovali kartu.";

        return Ok(response);
    }

    [HttpDelete("/Admin/[controller]/Delete/{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id, CancellationToken cancellationToken)
    {
        Response response = new();
        Ticket? ticket = await _ticketRepository.GetByIdAsync(id, cancellationToken);

        if (ticket is null)
        {
            response.Message = "Karta nije pronađena";

            return NotFound(response);
        }

        _ticketRepository.Delete(ticket);
        await _unitOfWork.CommitAsync(cancellationToken);

        response.Message = "Uspješno obrisana karta";

        return Ok(response);
    }
}
