using Application.Services.Abstractions.Interfaces.Checkout;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Invoices;
using Domain.Entities.Payment;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Exceptions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations.Checkout;

public sealed class CheckoutService : ICheckoutService
{
    private readonly IEmailService _emailService;
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITaxRepository _taxRepository;

    public CheckoutService(
        IEmailService emailService, 
        IIssuedTicketRepository issuedTicketRepository, 
        IInvoiceRepository invoiceRepository, 
        ITicketRepository ticketRepository,
        ITaxRepository taxRepository,
        IUnitOfWork unitOfWork)
    {
        _emailService = emailService;
        _issuedTicketRepository = issuedTicketRepository;
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _ticketRepository = ticketRepository;
        _taxRepository = taxRepository;
    }

    public async Task IssueTicketAsync(Invoice invoice, User user, CancellationToken cancellationToken = default)
    {
        IDictionary<Guid, Ticket> ticketCache = await _ticketRepository.GetAll()
            .ToDictionaryAsync(key => key.Id, value => value, cancellationToken);

        foreach (IssuedTicket issuedTicket in invoice.IssuedTickets)
        {
            issuedTicket.Ticket = ticketCache[issuedTicket.TicketId];
            await _issuedTicketRepository.CreateAsync(issuedTicket, cancellationToken);
        }

        await _unitOfWork.CommitAsync(cancellationToken);
        await _emailService.SendIssuedTicketsAsync(user, invoice, cancellationToken);
    }

    public async Task IssueInvoiceAsync(Invoice invoice, User user, CancellationToken cancellationToken = default)
    {
        Tax? tax = await _taxRepository.GetActiveAsync(cancellationToken);
        
        if (tax == null)
        {
            throw new DomainException("A tax must be set!");
        }

        invoice.Total = invoice.IssuedTickets.Select(issuedTicket => issuedTicket.Ticket.Price)
            .Sum();

        invoice.TotalWithoutTax = invoice.Total - (invoice.Total * tax.Percentage);

        invoice.Tax = tax;

        await _invoiceRepository.CreateAsync(invoice, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _emailService.SendInvoiceAsync(user, invoice, cancellationToken);
    }
}
