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

    public async Task<Guid?> Register(Korisnik user, string password, CancellationToken cancellationToken)
    {
        Tuple<byte[], string> passwordHashAndSalt = _hashingService
            .GeneratePasswordHashAndSalt(password);

        user.PasswordHash = passwordHashAndSalt.Item2;
        user.PasswordSalt = passwordHashAndSalt.Item1;

        Guid? userId = await _korisnikRepozitorij.Create(user);
        RegistracijskiToken? token = await _registracijskiTokenRepository.Create(user);

        await _emailService.SendRegistrationMail(user, token!.Token!, cancellationToken);

        return userId;
    }

    public async Task<bool> ActivateUserAccount(string tokenString)
    {
        var token = await _registracijskiTokenRepository.GetInactiveByTokenString(tokenString);

        if (token is null)
            return false;

        Korisnik? user = token.Korisnik;

        if (user is null)
            return false;

        user!.Aktivan = true;
        token.Aktiviran = true;

        await _registracijskiTokenRepository.Update(token);
        await _korisnikRepozitorij.Update(user);

        return true;
    }

    public Task Logout(Korisnik user)
    {
        throw new NotImplementedException();
    }
}
