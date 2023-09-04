using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Domain.Enums.OrderBy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Invoice;
using System.Linq.Expressions;

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
            Expression<Func<IssuedTicket, IssuedTicketHistoryDto>> selector = issuedTicket => new IssuedTicketHistoryDto
            {
                TicketName = issuedTicket.Ticket.Name,
                Price = issuedTicket.Ticket.Price,
                IssuedDate = issuedTicket.IssuedDate,
                StartStationName = issuedTicket.Route.StartStation.Name,
                EndStationName = issuedTicket.Route.EndStation.Name
            };

            Expression<Func<IssuedTicket, object>> orderBy = issuedTicket => issuedTicket.IssuedDate;

            var data = await _issuedTicketRepository.GetByUserIdAsync(userId, selector, orderBy, OrderBy.Descending, cancellationToken);
                
            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }
    }
}
