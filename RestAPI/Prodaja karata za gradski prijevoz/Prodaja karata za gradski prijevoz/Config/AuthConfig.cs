using Application.Config;
using System.Security.Claims;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static class AuthConfig
{
    public static void AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", o =>
        {
            o.Authority = builder.Configuration["Jwt:Authority"];
            o.Audience = builder.Configuration["Jwt:Audience"];
        });

        builder.Services.AddAuthorization(opt =>
        {
            opt.AddPolicy(AuthorizationPolicies.AdminPolicyName, config => config.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.UserPolicyName, config => config.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.UserPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.DriverPolicyName, config => config.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.DriverPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.AdminUserPolicyName, config 
                => config.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue, AuthorizationPolicies.UserPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.AdminDriverPolicyName, config => 
                config.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue, AuthorizationPolicies.DriverPolicyValue));
        });
    }
}
