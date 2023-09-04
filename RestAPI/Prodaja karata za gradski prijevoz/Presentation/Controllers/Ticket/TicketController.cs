using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Controllers.CardType;

[ApiController]
[Route("[controller]")]
public sealed class TicketController : ControllerBase
{
    private readonly ITicketRepository _ticketRepository;
    public TicketController(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllCardTypes(CancellationToken cancellationToken)
    {
        Ticket[] tickets = await _ticketRepository.GetAll()
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = tickets
        };
        
        return Ok(response);
         // todo: we will need a repository for card type creation and read which means we will need to remove the enum
    }
}
