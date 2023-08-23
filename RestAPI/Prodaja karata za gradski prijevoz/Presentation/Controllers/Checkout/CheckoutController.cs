using Application.Services.Abstractions.Interfaces.Checkout;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.DTO.Checkout;

namespace Presentation.Controllers.Checkout;

[ApiController]
[Authorize]
[Route("[controller]")]
public sealed class CheckoutController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(IUserRepository userRepository, ICheckoutService checkoutService, IPaymentMethodRepository paymentMethodRepository, ITicketRepository ticketRepository)
    {
        _userRepository = userRepository;
        _checkoutService = checkoutService;
        _paymentMethodRepository = paymentMethodRepository;
        _ticketRepository = ticketRepository;
    }

    [HttpPost("Finish")]
    public async Task<IActionResult> Checkout(CheckoutDto checkoutDto, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByIdAsync(checkoutDto.UserId, cancellationToken);

        if (user is null)
        {
            Response<string> badRequestReponse = new()
            {
                Message = "Cannot complete purchase as the user was not found. Try to login first."
            };

            return BadRequest(badRequestReponse);
        }

        bool doesTicketExtis = await _ticketRepository.DoesTicketExistAsync(checkoutDto.TicketId, cancellationToken);
        bool doesPaymentMethodExist = await _paymentMethodRepository.DoesPaymentMethodExistAsync(checkoutDto.PaymentMethodId, cancellationToken);

        if (!doesPaymentMethodExist || !doesTicketExtis)
        {
            Response<string> badResponse = new()
            {
                Message = "Faulty data. Please, try again."
            };

            return BadRequest(badResponse);
        }

        List<IssuedTicket> issuedTickets = new();

        for (int i = 0; i < checkoutDto.Quantity; ++i)
        {
            issuedTickets.Add(new()
            {
                Amount = 1,
                IssuedDate = DateTime.UtcNow,
                TicketId = checkoutDto.TicketId,
                User = user,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow
            });
        }

        Invoice invoice = new()
        {
            InvoicingDate = DateTime.UtcNow,
            IssuedTickets = issuedTickets,
            PaymentOptionId = checkoutDto.PaymentMethodId,
            User = user
        };

        await _checkoutService.IssueTicketAsync(invoice, user, cancellationToken);
        await _checkoutService.IssueInvoiceAsync(invoice, user, cancellationToken);

        Response<string> response = new()
        {
            Message = "Success! Your tickets and invoice will arrive in your email inbox shortly. Check your spam folder if you cannot find it."
        };

        return Ok(response);
    }
}
