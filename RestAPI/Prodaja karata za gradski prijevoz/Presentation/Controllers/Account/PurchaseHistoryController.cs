using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Invoice;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers.Account
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public sealed class PurchaseHistoryController : ControllerBase
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public PurchaseHistoryController(IInvoiceRepository invoiceRepository)
        {
            this._invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserPurchases(Guid userId, CancellationToken cancellationToken)
        {
            var data = await _invoiceRepository
                .GetAll()
                .Include(invoice => invoice.Ticket)
                .Where(invoice => invoice.UserId == userId)
                .Select(invoice => new InvoiceDto 
                { 
                    TicketName = invoice.Ticket.Name, 
                    Price=invoice.Price, 
                    PurchaseDate=invoice.PurchaseDate 
                })
                .Take(10)
                .ToArrayAsync(cancellationToken);

            Response<InvoiceDto[]> response = new()
            {
                Message = "Ok",
                Data = data
            };

            return Ok(response);
        } 
    }
}
