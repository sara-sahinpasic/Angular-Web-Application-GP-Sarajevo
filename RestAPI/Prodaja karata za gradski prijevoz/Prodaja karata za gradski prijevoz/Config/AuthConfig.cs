using Application.Config;
using System.Security.Claims;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static partial class AuthConfig
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
            opt.AddPolicy(AuthorizationPolicies.AdminPolicyName, builder => builder.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.UserPolicyName, builder => builder.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.UserPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.DriverPolicyName, builder => builder.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.DriverPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.AdminUserPolicyName, builder 
                => builder.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue, AuthorizationPolicies.UserPolicyValue));
            opt.AddPolicy(AuthorizationPolicies.AdminDriverPolicyName, builder => 
                builder.RequireClaim(ClaimTypes.Role, AuthorizationPolicies.AdminPolicyValue, AuthorizationPolicies.DriverPolicyValue));
        });
    }
}
