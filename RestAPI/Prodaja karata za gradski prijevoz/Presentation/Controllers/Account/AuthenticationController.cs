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
            Response errorResponse = new()
            {
                Message = "auth_controller_register_action_user_registered_error"
            };

            return BadRequest(errorResponse);
        }
        
        User user = new();
        objectMapperService.Map(userRequest, user);

        RegisterResult registerResult = await _authService.RegisterAsync(user, userRequest.Password!, cancellationToken);

        Response response = new()
        {
            Message = "auth_controller_register_action_success",
            Data = registerResult
        };

        return CreatedAtAction(nameof(RegisterAction), response);
    }

    [HttpPut("/account/activate/{tokenString}")]
    public async Task<IActionResult> ActivateAction(string tokenString, CancellationToken cancellationToken)
    {
        RegistrationToken? registrationToken = await _registrationTokenRepository.GetByTokenStringAsync(tokenString, cancellationToken);
        Response errorResponse;

        if (registrationToken is null)
        {
            errorResponse = new()
            {
                Message = "auth_controller_activate_action_invalid_token"
            };

            return BadRequest(errorResponse);
        }

        User? user = await _userRepository.GetByIdAsync(registrationToken.UserId, cancellationToken);

        if (await _authService.HasRegistrationTokenExpiredAsync(registrationToken, cancellationToken))
        {
            await _authService.ResendActivationCodeAsync(user!.Email!, cancellationToken);

            errorResponse = new()
            {
                Message = "auth_controller_activate_action_expired_token_error"
            };

            return BadRequest(errorResponse);
        }

        await _authService.ActivateUserAccountAsync(user!, registrationToken, cancellationToken);

        return Ok();
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LoginAction(
        UserLoginRequestDto loginData,
        CancellationToken cancellationToken)
    {
        Response errorResponse = new();
        
        if (string.IsNullOrEmpty(loginData.Password))
        {
            errorResponse.Message = "auth_controller_login_action_email_cannot_be_empty";
            return BadRequest(errorResponse);
        }

        if (string.IsNullOrEmpty(loginData.Email))
        {
            errorResponse.Message = "auth_controller_login_action_password_cannot_be_empty";
            return BadRequest(errorResponse);
        }

        LoginResult? loginResult = await _authService.LoginAsync(loginData.Email, loginData.Password, cancellationToken);


        if (loginResult is null)
        {
            errorResponse.Message = "auth_controller_login_action_user_not_found";
            return BadRequest(errorResponse);
        }

        if (!await _authService.IsUserActivatedAsync(loginData.Email, cancellationToken))
        {
            errorResponse.Message = "auth_controller_login_action_account_not_active";
            return BadRequest(errorResponse); // todo: create also if user exists in service
        }

        string message = loginResult.IsTwoWayAuth ? "auth_controller_login_action_two_way_auth" : "auth_controller_login_action_login_success";

        Response response = new()
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
        Response response = new();

        if (verificationCode is null)
        {
            response.Message = "auth_controller_authenticate_login_incorrect_login_code";
            return BadRequest(response);
        }

        if (_authService.HasAuthCodeExpired(verificationCode, cancellationToken))
        {
            response.Message = "auth_controller_authenticate_login_code_expired";
            return BadRequest(response);
        }

        string token = await _authService.AuthenticateLoginAsync(verificationCode, cancellationToken);

        response.Message = "auth_controller_login_action_login_success";
        response.Data = token;

        return Ok(response);
    }

    [HttpPost("/resetPassword")]
    public async Task<IActionResult> ResetPasswordAction([FromBody] string email, CancellationToken cancellationToken)
    {
        Response response = new();

        if (string.IsNullOrEmpty(email))
        {
            response.Message = "auth_controller_reset_password_error_empty_email_field";

            return BadRequest(response);
        }

        await _authService.ResetPasswordAsync(email, cancellationToken);

        response.Message = "auth_controller_reset_password_success";

        return Ok(response);
    }
}