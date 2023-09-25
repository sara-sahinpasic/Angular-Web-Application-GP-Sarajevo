using IdentityServer.Configuration;

var builder = WebApplication.CreateBuilder(args);

string clientSecret = builder.Configuration.GetValue<string>("ClientSecret");

IIdentityServerBuilder identityServerBuilder =  builder.Services.AddIdentityServer()
    .AddInMemoryClients(IdentityServerConfiguration.GetClients(clientSecret))
    .AddInMemoryApiScopes(IdentityServerConfiguration.ApiScopes)
    .AddInMemoryApiResources(IdentityServerConfiguration.ApiResources)
    .AddCustomTokenRequestValidator<ClaimsTokenRequestValidator>();

if (builder.Environment.IsDevelopment())
{
    identityServerBuilder.AddDeveloperSigningCredential();
}

var app = builder.Build();

app.UseIdentityServer();

app.Run();
