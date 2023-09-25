using IdentityServer4.Validation;
using System.Security.Claims;

namespace IdentityServer.Configuration;

public sealed class ClaimsTokenRequestValidator : ICustomTokenRequestValidator
{
    public Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        string? userClaims = context.Result.ValidatedRequest.Raw.Get("UserClaims");

        if (!string.IsNullOrEmpty(userClaims)) 
        {
            context.Result.ValidatedRequest.ClientClaims = new List<Claim> { new("userData", userClaims) };
        }

        return Task.CompletedTask;
    }
}
