using IdentityServer4.Validation;
using System.Security.Claims;
using System.Text.Json;

namespace IdentityServer.Configuration;

public sealed class ClaimsTokenRequestValidator : ICustomTokenRequestValidator
{
    public Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        string? userClaims = context.Result.ValidatedRequest.Raw.Get("UserClaims");

        if (!string.IsNullOrEmpty(userClaims)) 
        {
            Dictionary<string, string> claimsJson = JsonSerializer.Deserialize<Dictionary<string, string>>(userClaims)!;
            List<Claim> claims = new();

            foreach(KeyValuePair<string, string> item in claimsJson)
            {
                claims.Add(new(item.Key, item.Value));
            }

            context.Result.ValidatedRequest.ClientClaims = claims;
        }

        return Task.CompletedTask;
    }
}
