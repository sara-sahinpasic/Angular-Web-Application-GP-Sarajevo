using IdentityServer4.Models;

namespace IdentityServer.Configuration;

public static class IdentityServerConfiguration
{
    public static IEnumerable<Client> GetClients(string clientSecret)
    {
        return new List<Client>
        {
            new()
            {
                ClientId = "api",
                AllowedScopes = { "api.all" },
                ClientClaimsPrefix = "",
                AccessTokenLifetime = 1800,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AlwaysSendClientClaims = true,
                ClientSecrets = { new Secret(clientSecret.Sha256()) }
            }
        };
    }

    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
    {
        new("api.all")
    };

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
    {
        new("api")
        {
            Enabled = true,
            Scopes = new[] { "api.all" }
        }
    };
}
