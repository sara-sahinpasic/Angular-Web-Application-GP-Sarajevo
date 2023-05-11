using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace Application.Services.Implementations.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashingService _hashingService;
    private readonly IEmailService _emailService;
    private readonly IRegistrationTokenRepository _registrationTokenRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    // todo: generic response and request class: try and to in sprint 2
    public AuthService(
        IUserRepository userRepository,
        IEmailService emailService,
        IHashingService hashingService,
        IRegistrationTokenRepository registrationTokenRepository,
        IVerificationCodeRepository verificationCodeRepository,
        IUnitOfWork unitOfWork,
        IConfiguration config)
    {
        _userRepository = userRepository;
        _hashingService = hashingService;
        _emailService = emailService;
        _registrationTokenRepository = registrationTokenRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _config = config;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> HasAuthCodeExpiredAsync(VerificationCode verificationCode, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(verificationCode, nameof(verificationCode));

        return DateTime.Now.Millisecond - verificationCode?.DateExpiring.Millisecond > 1000 * 60 * 5 || verificationCode.Activated;
    }

    public async Task<string> AuthenticateLoginAsync(VerificationCode verificationCode, CancellationToken cancellationToken)
    {
        verificationCode.Activated = true;
        _verificationCodeRepository.Update(verificationCode);

        await _unitOfWork.CommitAsync(cancellationToken);

        string jwtToken = GenerateJwtToken(verificationCode.User);

        return jwtToken;
    }

    //todo: create factory: try to do in sprint 2
    public async Task<Guid?> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user is null || !_hashingService.VerifyPasswordHash(password, user.PasswordHash!, user.PasswordSalt!)) 
        {
            return null;
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

        return user.Id;
    }

    public async Task<Guid> RegisterAsync(User user, string password, CancellationToken cancellationToken)
    {
        Tuple<byte[], string> passwordHashAndSalt = _hashingService.GeneratePasswordHashAndSalt(password);

        user.PasswordHash = passwordHashAndSalt.Item2;
        user.PasswordSalt = passwordHashAndSalt.Item1;

        user.Id = Guid.NewGuid();
        _userRepository.Create(user);

        RegistrationToken token = new()
        {
            User = user,
            Token = GenerateRegistrationToken(), // todo: new algorithm: sprint 2
        };

        _registrationTokenRepository.Create(token);
        await _unitOfWork.CommitAsync(cancellationToken);

        await _emailService.SendRegistrationMailAsync(user, token!.Token!, cancellationToken);

        return user.Id;
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

    public async Task ActivateUserAccountAsync(User user, RegistrationToken registrationToken, CancellationToken cancellationToken)
    {
        user!.Active = true;
        registrationToken.IsActivated = true;

        _userRepository.Update(user);
        _registrationTokenRepository.Update(registrationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }

    public Task LogoutAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserActivatedAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);

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
        var user = await _userRepository.GetByEmailAsync(email);

        await _emailService.SendRegistrationMailAsync(user, GenerateRegistrationToken(), cancellationToken);
    }

    private int GenerateVerificationCode()
    {
        return Random.Shared.Next(1000, 9999);
    }

    private string GenerateJwtToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        object userProfile = new
        {
            id = user.Id,
            firstName = user.FirstName,
            lastName = user.LastName,
            address = user.Address,
            email = user.Email,
            dateOfBirth = user.DateOfBirth,
            phoneNumber = user.PhoneNumber
        };

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, JsonSerializer.Serialize(userProfile)),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _config["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = _config["Jwt:Issuer"],
            Audience = _config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        string jwtToken = tokenHandler.WriteToken(token);

        return jwtToken;
    }

    private string GenerateRegistrationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("%", ".").Replace("<", ".")
            .Replace(">", ".").Replace("{", "-").Replace("}", "_").Replace("?", ".").Replace("#", ".")
            .Replace("=", ".");
    }
}
