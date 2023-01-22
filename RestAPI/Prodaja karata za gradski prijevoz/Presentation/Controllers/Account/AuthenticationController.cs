using Application.Services.Abstractions.Interfaces.Mapper;
using Presentation.DTO.Korisnik;
using Domain.Entities.Korisnici;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions.Interfaces.Authentication;

namespace Presentation.Controllers.Account;

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

        return CreatedAtAction(nameof(RegisterAction), userId);
    }

    [HttpGet("/account/activate/{tokenString}")]
    public async Task<IActionResult> ActivateAction(string tokenString)
    {
        bool success = await _authService.ActivateUserAccount(tokenString);

        if (!success)
            return BadRequest("Token is either already activated or it is expired.");

        return Ok("Account activated!");
    }
}