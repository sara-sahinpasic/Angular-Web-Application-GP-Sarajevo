using Application.DataClasses.User;
using Domain.Entities.Users;
using System.Text.Json;

namespace Application.Services.Abstractions.Interfaces.Authentication;

public interface IAuthService
{
    /// <summary>
    /// Verifies that the verification code isn't expired and that it is correct and logs in the user
    /// </summary>
    /// <param name="loginCode">Verification code</param>
    /// <param name="userId">User Id</param>
    /// <returns>Jwt token when task is done</returns>
    Task<LoginResult?> AuthenticateLoginAsync(VerificationCode verificationCode, CancellationToken cancellationToken);
    bool HasAuthCodeExpired(VerificationCode verificationCode, CancellationToken cancellationToken);
    /// <summary>
    /// Verifies the password and email and sends verification code to user
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <param name="password">Password of the user</param>
    /// <returns>Returns a user id when EnableTwoWayAuth is set to true in appsettings, otherwise returns a jwt token with user data</returns>
    Task<LoginResult?> LoginAsync(string email, string password, CancellationToken cancellationToken);
    /// <summary>
    /// Creates the user and hashes the passsword.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>User ID if the user email is not registered in the database. Null if the email is already registered.</returns>
    Task<RegisterResult> RegisterAsync(User user, string password, CancellationToken cancellationToken);
    Task<bool> HasRegistrationTokenExpiredAsync(RegistrationToken registrationToken, CancellationToken cancellationToken);
    Task ActivateUserAccountAsync(User user, RegistrationToken registrationToken, CancellationToken cancellationToken);
    Task<bool> IsUserActivatedAsync(string emai, CancellationToken cancellationToken);
    Task ResendVerificationCodeAsync(User user, CancellationToken cancellationToken);
    Task ResendActivationCodeAsync(string email, CancellationToken cancellationToken);
    Task ResetPasswordAsync(string email, CancellationToken cancellationToken);
    Task<JsonElement> GetAuthTokenAsync(User user, CancellationToken cancellationToken = default);
}
