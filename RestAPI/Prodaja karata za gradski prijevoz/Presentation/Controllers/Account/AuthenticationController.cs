using Application.Services.Abstractions.Interfaces.Mapper;
using Presentation.DTO.Korisnik;
using Domain.Entities.Korisnici;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers.Account;

// todo: return message for account already existing on register
[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthService _authService;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    // todo: change bools to custom exceptions that are handled on the error controller handler
    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAction
    (
        [FromServices] IObjectMapperService objectMapperService,
        KorisnikRequest userRequest,
        CancellationToken cancellationToken
    )
    {
        if (userRequest is null)
            return BadRequest("User cannot be null!");

        Korisnik user = new();
        objectMapperService.Map(userRequest, user);

        Guid? userId = await _authService.Register(user, userRequest.Password!, cancellationToken);

        if (userId is null)
            return BadRequest("User already exists!");

        return CreatedAtAction(nameof(RegisterAction), userId);
    }

    // todo: create login code page. Some verification code cleanup. Create resend activation mail route.
    [HttpPut("/account/activate/{tokenString}")]
    public async Task<IActionResult> ActivateAction(string tokenString, CancellationToken cancellationToken)
    {
        bool success = await _authService.ActivateUserAccount(tokenString, cancellationToken);

        if (!success)
            return BadRequest("Token is either already activated or it is expired. Check your email for a new token.");

        return Ok();
    }
}