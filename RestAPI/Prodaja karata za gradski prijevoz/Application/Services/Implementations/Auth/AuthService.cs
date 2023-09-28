using Application.DataClasses.User;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Prodaja_karata_za_gradski_prijevoz.Config;
using System.Text.Json;

namespace Application.Services.Implementations.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;
    private readonly IRegistrationTokenRepository _registrationTokenRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly AuthConfirmationConfig _authConfirmationConfig;

    public AuthService(
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordService hashingService,
        IRegistrationTokenRepository registrationTokenRepository,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork,
        IConfiguration config,
        HttpClient httpClient,
        AuthConfirmationConfig authConfirmationConfig)
    {
        _userRepository = userRepository;
        _passwordService = hashingService;
        _emailService = emailService;
        _registrationTokenRepository = registrationTokenRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _config = config;
        _unitOfWork = unitOfWork;
        _authConfirmationConfig = authConfirmationConfig;
        _httpClient = httpClient;
    }

    public bool HasAuthCodeExpired(VerificationCode verificationCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(verificationCode, nameof(verificationCode));

        return DateTime.Now.Millisecond - verificationCode?.DateExpiring.Millisecond > 1000 * 60 * 5 || verificationCode.Activated;
    }

    public async Task<LoginResult?> AuthenticateLoginAsync(VerificationCode verificationCode, CancellationToken cancellationToken)
    {
        verificationCode.Activated = true;
        _verificationCodeRepository.Update(verificationCode);

        await _unitOfWork.CommitAsync(cancellationToken);

        JsonElement authToken = await GetAuthTokenAsync(verificationCode.User, cancellationToken);

        return new LoginResult
        {
            LoginData = authToken,
            IsTwoWayAuth = _authConfirmationConfig.ShouldUseTwoWayAuth
        };
    }

    public async Task<LoginResult?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken, new string[] { "Role" });
        
        if (user is null || !_passwordService.VerifyPasswordHash(password, user.PasswordHash!, user.PasswordSalt!)) 
        {
            return null;
        }

        if (!_authConfirmationConfig.ShouldUseTwoWayAuth)
        {
            JsonElement authToken = await GetAuthTokenAsync(user, cancellationToken);

            return new LoginResult
            {
                LoginData = authToken,
                IsTwoWayAuth = _authConfirmationConfig.ShouldUseTwoWayAuth
            };
        }

        VerificationCode? oldCode = await _verificationCodeRepository.GetByUserIdAsync(user.Id, cancellationToken);

        if (oldCode is not null) 
        {
            _verificationCodeRepository.Delete(oldCode);
        }

        VerificationCode verificationCode = new()
        {
            UserId = user.Id,
            Code = GenerateVerificationCode(),
            DateCreated = DateTime.UtcNow,
            DateExpiring = DateTime.UtcNow.AddMinutes(5)
        };

        _verificationCodeRepository.Create(verificationCode);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _emailService.SendLoginVerificationMailAsync(user, verificationCode.Code, cancellationToken);

        return new LoginResult
        {
            LoginData = user.Id.ToString(),
            IsTwoWayAuth = _authConfirmationConfig.ShouldUseTwoWayAuth
        };
    }

    private async Task<TokenResponse?> GetIdentityFromIdentityServer(UserClaims userClaims, CancellationToken cancellationToken = default)
    {
        string identityServerUrl = _config["Jwt:Authority"];
        DiscoveryDocumentResponse discoveryDocument = await _httpClient.GetDiscoveryDocumentAsync(identityServerUrl, cancellationToken);

        string userData = JsonSerializer.Serialize(userClaims, options: new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        TokenResponse? tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = "api",
            ClientSecret = _config["Jwt:Secret"],
            Scope = "api.all",
            Parameters = new Parameters()
            {
                new KeyValuePair<string, string>("UserClaims", userData)
            }
        }, cancellationToken);

        return tokenResponse;
    }

    public async Task<JsonElement> GetAuthTokenAsync(User user, CancellationToken cancellationToken = default)
    {
        UserClaims userClaims = GetUserClaimsData(user);
        TokenResponse? tokenResponse = await GetIdentityFromIdentityServer(userClaims, cancellationToken);

        if (tokenResponse is null || tokenResponse.IsError)
        {
            throw new HttpRequestException(tokenResponse?.Error);
        }

        return tokenResponse.Json!;
    }

    public async Task<RegisterResult> RegisterAsync(User user, string password, CancellationToken cancellationToken)
    {
        Tuple<byte[], string> passwordHashAndSalt = _passwordService.GeneratePasswordHashAndSalt(password);

        user.PasswordHash = passwordHashAndSalt.Item2;
        user.PasswordSalt = passwordHashAndSalt.Item1;

        user.Id = Guid.NewGuid();
        _userRepository.Create(user);

        RegistrationToken? token = null;
        bool shouldActivateUserOnCreation = true;

        if (_authConfirmationConfig.ShoudUseRegisteredAccountConfirmation)
        {
            shouldActivateUserOnCreation = false;
            token = new()
            {
                User = user,
                Token = GenerateRegistrationToken(), // todo: new algorithm: sprint 2
            };

            _registrationTokenRepository.Create(token);
        }

        user.Active = shouldActivateUserOnCreation;

        await _unitOfWork.CommitAsync(cancellationToken);

        // done this way because we want to send the email after succesfully commiting all changes to the database
        if (_authConfirmationConfig.ShoudUseRegisteredAccountConfirmation)
        {
            await _emailService.SendRegistrationMailAsync(user, token!.Token!, cancellationToken);
        }

        return new RegisterResult()
        {
            IsAccountActivationRequired = _authConfirmationConfig.ShoudUseRegisteredAccountConfirmation,
            UserId = user.Id,
        };
    }

    public async Task<bool> HasRegistrationTokenExpiredAsync(RegistrationToken registrationToken, CancellationToken cancellationToken)
    {
        if (DateTime.Now.Millisecond - registrationToken.CreatedDate.Millisecond < 1000 * 60 * 30)
        {
            return false;
        }

        registrationToken.IsExpired = true;

        _registrationTokenRepository.Update(registrationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return true;
    }

    public Task ActivateUserAccountAsync(User user, RegistrationToken registrationToken, CancellationToken cancellationToken)
    {
        user!.Active = true;
        registrationToken.IsActivated = true;

        _userRepository.Update(user);
        _registrationTokenRepository.Update(registrationToken);

        return _unitOfWork.CommitAsync(cancellationToken);
    }

    public async Task<bool> IsUserActivatedAsync(string email, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        return user is not null && user.Active;
    }

    public async Task ResendVerificationCodeAsync(User user, CancellationToken cancellationToken)
    {
        VerificationCode newVerificationCode = new()
        {
            UserId = user!.Id,
            Code = GenerateVerificationCode(),
            DateCreated = DateTime.UtcNow,
            DateExpiring = DateTime.UtcNow.AddMinutes(5)
        };

        _verificationCodeRepository.Create(newVerificationCode);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _emailService.SendLoginVerificationMailAsync(user, newVerificationCode.Code, cancellationToken);
    }

    public async Task ResendActivationCodeAsync(string email, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        await _emailService.SendRegistrationMailAsync(user, GenerateRegistrationToken(), cancellationToken);
    }

    private int GenerateVerificationCode()
    {
        return Random.Shared.Next(1000, 9999);
    }

    private UserClaims GetUserClaimsData(User user)
    {
        UserClaims userClaims = new()
        {
            Id = user.Id,
            Role = user.Role.Name,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Address = user.Address,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            PhoneNumber = user.PhoneNumber
        };

        return userClaims;
    }

    private string GenerateRegistrationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("%", ".").Replace("<", ".")
            .Replace(">", ".").Replace("{", "-").Replace("}", "_").Replace("?", ".").Replace("#", ".")
            .Replace("=", ".");
    }

    public async Task ResetPasswordAsync(string email, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return;
        }

        string newPassword = _passwordService.GenerateRandomPassword();

        Tuple<byte[], string> passwordParts = _passwordService.GeneratePasswordHashAndSalt(newPassword);

        string passwordHash = passwordParts.Item2;
        byte[] salt = passwordParts.Item1;

        user.PasswordHash = passwordHash;
        user.PasswordSalt = salt;

        await _unitOfWork.CommitAsync(cancellationToken);

        string subject = "Reset password";
        string content = $"Your new password is {newPassword}. Please, change your password the next time you log in.";

        await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);
    }
}
