using Application.Services.Abstractions.Interfaces.Mapper;
using Presentation.DTO.Korisnik;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.Services.Abstractions.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Presentation.DTO.User;

namespace Presentation.Controllers.Account;

[ApiController]
[Route("[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IAuthService _authService;
    private readonly IRegistrationTokenRepository _registrationTokenRepository;
    private readonly IUserRepository _userRepository;

    public AuthenticationController(ILogger<AuthenticationController> logger, IAuthService authService, IRegistrationTokenRepository registrationTokenRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _authService = authService;
        _registrationTokenRepository = registrationTokenRepository;
        _userRepository = userRepository;
    }

    [HttpPost("/register")]
    public async Task<IActionResult> RegisterAction
    (
        [FromServices] IObjectMapperService objectMapperService, KorisnikRequest userRequest,
        CancellationToken cancellationToken
    )
    {
        if (userRequest is null)
            return BadRequest("User cannot be null!");

        User user = new();
        objectMapperService.Map(userRequest, user);

        Guid userId = await _authService.RegisterAsync(user, userRequest.Password!, cancellationToken);

        return CreatedAtAction(nameof(RegisterAction), userId);
    }

    [HttpPut("/account/activate/{userId}/{tokenString}")]
    public async Task<IActionResult> ActivateAction(Guid userId, string tokenString, CancellationToken cancellationToken)
    {
        RegistrationToken? registrationToken = await _registrationTokenRepository.GetByTokenStringAsync(tokenString, cancellationToken);
        User? user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (await _authService.HasRegistrationTokenExpiredAsync(registrationToken, cancellationToken))
        {
            await _authService.ResendActivationCodeAsync(user.Email, cancellationToken);

            return BadRequest("Token has expired. A new one has been sent.");
        }

        await _authService.ActivateUserAccountAsync(user, registrationToken, cancellationToken);

        return Ok();
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LoginAction(
        UserLoginRequestDto loginData,
        CancellationToken cancellationToken)
    {
        if (!await _authService.IsUserActivatedAsync(loginData.Email, cancellationToken))
            return BadRequest("Account not activated. Check your email for the activation code.");

        Guid? userId = await _authService.LoginAsync(loginData.Email, loginData.Password, cancellationToken);

        return Ok(new
        {
            Message = "Verification code sent to email",
            UserId = userId
        });
    }

    [HttpPost("/verifyLogin")]
    public async Task<IActionResult> AuthenticateLoginAction(
        [FromServices] IVerificationCodeRepository verificationCodeRepository, AuthLoginDataDto authLoginData, 
        CancellationToken cancellationToken)
    {
        VerificationCode? verificationCode = await verificationCodeRepository.GetByUserIdAndCodeAsync(authLoginData.UserId, authLoginData.Code, cancellationToken);

        if (verificationCode is null)
        {
            return BadRequest("Login code not correct. Try again.");
        }

        if (await _authService.HasAuthCodeExpiredAsync(verificationCode, cancellationToken))
        {
            return BadRequest("Login code expired.");
        }

        string token = await _authService.AuthenticateLoginAsync(verificationCode, cancellationToken);

        return Ok(new
        {
            Token = token
        });
    }
}