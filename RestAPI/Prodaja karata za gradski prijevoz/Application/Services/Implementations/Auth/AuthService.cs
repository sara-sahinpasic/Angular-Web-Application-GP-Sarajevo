using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Domain.Entities.Korisnici;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Implementations.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IKorisnikRepozitorij _korisnikRepozitorij;
    private readonly IHashingService _hashingService;
    private readonly IEmailService _emailService;
    private readonly IRegistracijskiTokenRepository _registracijskiTokenRepository;
    private readonly IVerificationCodeRepository _verificationCodeRepository;
    private readonly IConfiguration _config;

    // todo: create a generic repository
    // todo: custom exceptions so that they can be thrown and caught on the error controller
    // todo: generic response and request class
    public AuthService(
        IKorisnikRepozitorij korisnikRepozitorij,
        IEmailService emailService,
        IHashingService hashingService,
        IRegistracijskiTokenRepository registracijskiTokenRepository,
        IVerificationCodeRepository verificationCodeRepository,
        IConfiguration config)
    {
        _korisnikRepozitorij = korisnikRepozitorij;
        _hashingService = hashingService;
        _emailService = emailService;
        _registracijskiTokenRepository = registracijskiTokenRepository;
        _verificationCodeRepository = verificationCodeRepository;
        _config = config;
    }

    public async Task<string> AuthenticateLogin(Guid userId, int loginCode, CancellationToken cancellationToken)
    {
        VerificationCode? code = await _verificationCodeRepository.GetByUserIdAndCode(userId, loginCode);

        if (code is null)
            return null;

        Korisnik? user = code?.User;

        if (DateTime.Now.Millisecond - code?.DateExpiring.Millisecond > 1000 * 60 * 5 || code.Activated)
        {
            VerificationCode verificationCode = new()
            {
                UserId = user!.Id,
                Code = GenerateVerificationCode(),
                DateCreated = DateTime.UtcNow,
                DateExpiring = DateTime.UtcNow.AddMinutes(5)
            };

            await _verificationCodeRepository.CreateAsync(verificationCode);
            await ResendVerificationCode(user!, verificationCode.Code, cancellationToken);
            
            return null;
        }

        code.Activated = true;
        await _verificationCodeRepository.UpdateAsync(code);

        string jwtToken = GenerateJwtToken(user!);

        return jwtToken;
    }
    //todo: create factory
    public async Task<Guid?> Login(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _korisnikRepozitorij.GetByEmail(email);
        
        if (user is not null && _hashingService.VerifyPasswordHash(password, user.PasswordHash!, user.PasswordSalt!)) 
        {
            VerificationCode? oldCode = await _verificationCodeRepository.GetByUserIdAsync(user.Id);

            if (oldCode is not null) 
            {
                await _verificationCodeRepository.DeleteAsync(oldCode);
            }

            VerificationCode verificationCode = new()
            {
                UserId = user.Id,
                Code = GenerateVerificationCode(),
                DateCreated = DateTime.UtcNow,
                DateExpiring = DateTime.UtcNow.AddMinutes(5)
            };

            await _emailService.SendLoginVerificationMail(user, verificationCode.Code, cancellationToken);
            await _verificationCodeRepository.CreateAsync(verificationCode);

            return user.Id;
        }

        return null;
    }

    public async Task<Guid?> Register(Korisnik user, string password, CancellationToken cancellationToken)
    {
        Tuple<byte[], string> passwordHashAndSalt = _hashingService
            .GeneratePasswordHashAndSalt(password);

        user.PasswordHash = passwordHashAndSalt.Item2;
        user.PasswordSalt = passwordHashAndSalt.Item1;

        user.Email = user.Email!.ToLower();

        Guid? userId = await _korisnikRepozitorij.Create(user);

        RegistracijskiToken token = new()
        {
            Korisnik = user,
            Token = GenerateRegistrationToken(), // todo: new algorithm
        };

        await _registracijskiTokenRepository.Create(token);
        await _emailService.SendRegistrationMail(user, token!.Token!, cancellationToken);

        return userId;
    }

    public async Task<bool> ActivateUserAccount(string tokenString, CancellationToken cancellationToken)
    {
        var token = await _registracijskiTokenRepository.GetInactiveByTokenString(tokenString);

        if (token is null)
            return false;

        Korisnik? user = token.Korisnik;

        if (user is null)
            return false;

        if (token.Aktiviran)
            return false;

        if (DateTime.Now.Millisecond - token.Kreiran.Millisecond > 1000 * 60 * 30)
        {
            token.Istekao = true;
            await _registracijskiTokenRepository.Update(token);

            await ResendActivationCode(user.Email, cancellationToken);
            return false;
        }

        user!.Aktivan = true;
        token.Aktiviran = true;

        await _korisnikRepozitorij.Update(user);
        await _registracijskiTokenRepository.Update(token);

        return true;
    }

    public Task Logout(Korisnik user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserActivated(string email)
    {
        var user = await _korisnikRepozitorij.GetByEmail(email);

        return user.Aktivan;
    }
    
    public async Task ResendActivationCode(string email, CancellationToken cancellationToken)
    {
        var user = await _korisnikRepozitorij.GetByEmail(email);

        await _emailService.SendRegistrationMail(user, GenerateRegistrationToken(), cancellationToken);
    }

    private int GenerateVerificationCode()
    {
        return Random.Shared.Next(1000, 9999);
    }

    private string GenerateJwtToken(Korisnik user)
    {
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.Ime} {user.Prezime}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
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

    private async Task ResendVerificationCode(Korisnik user, int code, CancellationToken cancellationToken)
    {
        await _emailService.SendLoginVerificationMail(user, code, cancellationToken);
    }
}
