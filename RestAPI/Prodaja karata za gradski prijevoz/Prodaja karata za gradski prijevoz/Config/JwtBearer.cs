using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static partial class JwtBearer
{
    public static void AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", o =>
        {
            o.Authority = builder.Configuration["Jwt:Authority"];
            o.Audience = builder.Configuration["Jwt:Audience"];
        });

        builder.Services.AddAuthorization();
    }
}
