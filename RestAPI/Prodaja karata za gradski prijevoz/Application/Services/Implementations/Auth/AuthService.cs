using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Domain.Entities.Korisnici;

namespace Application.Services.Implementations.Auth;

public sealed class AuthService : IAuthService
{
    private readonly IKorisnikRepozitorij _korisnikRepozitorij;
    private readonly IHashingService _hashingService;
    private readonly IEmailService _emailService;
    private readonly IRegistracijskiTokenRepository _registracijskiTokenRepository;

    
    public AuthService(
        IKorisnikRepozitorij korisnikRepozitorij,
        IEmailService emailService,
        IHashingService hashingService,
        IRegistracijskiTokenRepository registracijskiTokenRepository)
    {
        _korisnikRepozitorij = korisnikRepozitorij;
        _hashingService = hashingService;
        _emailService = emailService;
        _registracijskiTokenRepository = registracijskiTokenRepository;
    }

    public async Task<string> AuthenticateLogin(Guid userId, int loginCode, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    //todo: create factory
    public async Task<Guid?> Login(string email, string password, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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

    
    private string GenerateRegistrationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_").Replace("%", ".").Replace("<", ".")
            .Replace(">", ".").Replace("{", "-").Replace("}", "_").Replace("?", ".").Replace("#", ".")
            .Replace("=", ".");
    }
}
