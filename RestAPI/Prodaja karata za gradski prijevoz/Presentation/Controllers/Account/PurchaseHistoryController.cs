using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Invoice;
using Microsoft.EntityFrameworkCore;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;

namespace Presentation.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public sealed class PurchaseHistoryController : ControllerBase
    {
        private readonly IIssuedTicketRepository _issuedTicketRepository;

        public PurchaseHistoryController(IIssuedTicketRepository issuedTicketRepository)
        {
            _issuedTicketRepository = issuedTicketRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserPurchases(Guid userId, CancellationToken cancellationToken)
        {
            var data = await _issuedTicketRepository
                .GetAll()
                .Include(issuedTicket => issuedTicket.Ticket)
                .Where(issuedTicket => issuedTicket.UserId == userId)
                .Select(issuedTicket => new IssuedTicketHistoryDto
                { 
                    TicketName = issuedTicket.Ticket.Name, 
                    Price = issuedTicket.Ticket.Price, 
                    IssuedDate = issuedTicket.IssuedDate
                })
                .Take(10)
                .ToArrayAsync(cancellationToken);

            Response<IssuedTicketHistoryDto[]> response = new()
            {
                Message = "Ok",
                Data = data
            };

            return Ok(response);
        } 
    }
}
