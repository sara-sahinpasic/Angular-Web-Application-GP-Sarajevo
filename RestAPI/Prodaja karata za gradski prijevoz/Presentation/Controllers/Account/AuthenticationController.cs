using Application.DataClasses.User;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.DTO;
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
        [FromServices] IObjectMapperService objectMapperService, UserRegistrationRequestDto userRequest,
        CancellationToken cancellationToken
    )
    {
        if (await _userRepository.IsUserRegisteredAsync(userRequest.Email))
        {
            Response<string> errorResponse = new()
            {
                Message = "User is already registered!"
            };

            return BadRequest(errorResponse);
        }
        
        User user = new();
        objectMapperService.Map(userRequest, user);

        RegisterResult registerResult = await _authService.RegisterAsync(user, userRequest.Password!, cancellationToken);

        Response<RegisterResult> response = new()
        {
            Message = "User succesfully created",
            Data = registerResult
        };

        return CreatedAtAction(nameof(RegisterAction), response);
    }

    [HttpPut("/account/activate/{tokenString}")]
    public async Task<IActionResult> ActivateAction(string tokenString, CancellationToken cancellationToken)
    {
        RegistrationToken? registrationToken = await _registrationTokenRepository.GetByTokenStringAsync(tokenString, cancellationToken);

        if (registrationToken is null)
        {
            return BadRequest("Token not valid");
        }

        User? user = await _userRepository.GetByIdAsync(registrationToken.UserId, cancellationToken);

        if (await _authService.HasRegistrationTokenExpiredAsync(registrationToken, cancellationToken))
        {
            await _authService.ResendActivationCodeAsync(user!.Email!, cancellationToken);

            return BadRequest("Token has expired. A new one has been sent.");
        }

        await _authService.ActivateUserAccountAsync(user!, registrationToken, cancellationToken);

        return Ok();
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LoginAction(
        UserLoginRequestDto loginData,
        CancellationToken cancellationToken)
    {
        LoginResult? loginResult = await _authService.LoginAsync(loginData.Email, loginData.Password, cancellationToken);

        Response<string> errorResponse = new();

        if (loginResult is null)
        {
            errorResponse.Message = "User with those credentials not found.";
            return BadRequest(errorResponse);
        }

        if (!await _authService.IsUserActivatedAsync(loginData.Email, cancellationToken))
        {
            errorResponse.Message = "Account not activated. Check your email for the activation code.";
            return BadRequest(errorResponse); // todo: create also if user exists in service
        }

        string message = loginResult.IsTwoWayAuth ? "Verification code sent to email" : "Successfully logged in.";

        Response<LoginResult> response = new()
        {
            Message = message,
            Data = loginResult
        };

        return Ok(response);
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

        if (_authService.HasAuthCodeExpired(verificationCode, cancellationToken))
        {
            return BadRequest("Login code expired.");
        }

        string token = await _authService.AuthenticateLoginAsync(verificationCode, cancellationToken);

        Response<string> response = new()
        {
            Message = "Success",
            Data = token
        };

        return Ok(response);
    }

    [HttpPost("/resetPassword")]
    public async Task<IActionResult> ResetPasswordAction([FromBody] string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
        {
            Response<string> badResponse = new()
            {
                Message = "The email field cannot be empty."
            };

            return BadRequest(badResponse);
        }

        await _authService.ResetPasswordAsync(email, cancellationToken);

        Response<string> response = new()
        {
            Message = "Success",
            Data = "You can check your email now for a new password"
        };

        return Ok(response);
    }
}