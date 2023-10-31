using Application.Services.Abstractions.Interfaces.Checkout;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Implementations.Checkout;
using Domain.Entities.Invoices;
using Domain.Entities.Payment;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Enums.PaymentOption;
using Domain.Exceptions.Domain;
using FakeItEasy;
using FluentAssertions;

namespace UnitTests.Services.Application.Checkout;

public sealed class CheckoutServiceTests
{
    private readonly ICheckoutService _checkoutService;
    private readonly ITaxRepository _taxRepository;
    private readonly IEmailService _emailService;
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheckoutServiceTests()
    {
        _taxRepository = A.Dummy<ITaxRepository>();
        _emailService = A.Dummy<IEmailService>();
        _issuedTicketRepository = A.Dummy<IIssuedTicketRepository>();
        _invoiceRepository = A.Dummy<IInvoiceRepository>();
        _ticketRepository = A.Dummy<ITicketRepository>();
        _unitOfWork = A.Dummy<IUnitOfWork>();

        _checkoutService = new CheckoutService(_emailService, _issuedTicketRepository, _invoiceRepository, _ticketRepository, _taxRepository, _unitOfWork);
    }

    [Fact]
    public async Task IssueInvoiceAsync_ShouldThrowDomainException_WhenThereIsNoTaxActive()
    {
        Invoice invoice = A.Dummy<Invoice>();
        User user = A.Dummy<User>();

        A.CallTo(() => _taxRepository.GetActiveAsync(default)).Returns(Task.FromResult<Tax?>(null));

        Func<Task> act = async () => await _checkoutService.IssueInvoiceAsync(invoice, user);

        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task IssueInvoiceAsync_ShouldHaveCorrectTotalAndTotalWithoutTaxCalculation_WhenTaxAndTicketsExist()
    {
        User user = A.Dummy<User>();
        IEnumerable<IssuedTicket> issuedTickets = new List<IssuedTicket>()
        {
            new()
            {
                Amount = 1,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Name = "Test",
                    Price = 3.2,
                },
                User = user
            },
            new()
            {
                Amount = 1,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Name = "Test",
                    Price = 3.2,
                },
                User = user
            },
            new()
            {
                Amount = 1,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Name = "Test",
                    Price = 3.2,
                },
                User = user
            },
            new()
            {
                Amount = 1,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Name = "Test",
                    Price = 103.2,
                },
                User = user
            }
        };
        Invoice invoice = new()
        {
            InvoicingDate = DateTime.UtcNow,
            IssuedTickets = issuedTickets,
            PaymentOption = PaymentOptions.Card,
            Total = issuedTickets.Sum(issuedTicket => issuedTicket.Ticket.Price),
            User = user
        };

        Tax tax = new()
        {
            Percentage = 0.17,
            Name = "TestTax",
            Active = true
        };

        A.CallTo(() => _taxRepository.GetActiveAsync(default)).Returns(Task.FromResult<Tax?>(tax));
        A.CallTo(() => _emailService.SendInvoiceAsync(user, invoice, default)).Returns(Task.CompletedTask);
        A.CallTo(() => _invoiceRepository.CreateAsync(invoice, default)).Returns(Task.FromResult<Guid?>(invoice.Id));
        A.CallTo(() => _unitOfWork.CommitAsync(default)).Returns(Task.CompletedTask);

        await _checkoutService.IssueInvoiceAsync(invoice, user);

        double total = issuedTickets.Sum(issuedTicket => issuedTicket.Ticket.Price);
        double totalWithoutTax = total - (total * tax.Percentage);

        invoice.TotalWithoutTax.Should().BeApproximately(totalWithoutTax, 2);
        invoice.Total.Should().BeApproximately(total, 2);
    }
}
