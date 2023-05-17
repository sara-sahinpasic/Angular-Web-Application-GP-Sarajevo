using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Verifies that the verification code isn't expired and that it is correct and logs in the user
    /// </summary>
    /// <param name="loginCode">Verification code</param>
    /// <param name="userId">User Id</param>
    /// <returns>Jwt token when task is done</returns>
    Task<string> AuthenticateLoginAsync(VerificationCode verificationCode, CancellationToken cancellationToken);
    Task<bool> HasAuthCodeExpiredAsync(VerificationCode verificationCode, CancellationToken cancellationToken);
    /// <summary>
    /// Verifies the password and email and sends verification code to user
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <param name="password">Password of the user</param>
    /// <returns>True if login is successfull, false if not</returns>
    Task<Guid?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    /// <summary>
    /// Creates the user and hashes the passsword.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>User's ID if the user email is not registered in the database. Null if the email is already registered.</returns>
    Task<Guid> RegisterAsync(User user, string password, CancellationToken cancellationToken);
    Task<bool> HasRegistrationTokenExpiredAsync(RegistrationToken registrationToken, CancellationToken cancellationToken);
    Task ActivateUserAccountAsync(User user, RegistrationToken registrationToken, CancellationToken cancellationToken);
    Task LogoutAsync(User user, CancellationToken cancellationToken);
    Task<bool> IsUserActivatedAsync(string emai, CancellationToken cancellationToken);
    Task ResendVerificationCodeAsync(User user, CancellationToken cancellationToken);
    Task ResendActivationCodeAsync(string email, CancellationToken cancellationToken);
    Task ResetPasswordAsync(string email, CancellationToken cancellationToken);
}
