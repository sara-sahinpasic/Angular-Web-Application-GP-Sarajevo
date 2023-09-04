using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Domain.Entities.Payment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;

namespace Presentation.Controllers.Payment;

[ApiController]
[Route("[controller]")]
public sealed class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllPaymentMethods(CancellationToken cancellationToken)
    {
        PaymentMethod[] paymentMethods = await _paymentMethodRepository.GetAll()
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = paymentMethods
        };

        return Ok(response);
    }
}
