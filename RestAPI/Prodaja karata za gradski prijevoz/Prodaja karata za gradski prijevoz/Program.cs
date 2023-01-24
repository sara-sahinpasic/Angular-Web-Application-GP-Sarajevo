using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Application.Services.Implementations.Hashing;
using Application.Services.Implementations.Mapper;
using Infrastructure.Data;
using Infrastructure.Repositories.Korisnici;
using Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Implementations.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddApplicationPart(Presentation.PresentationAssembly.Assembly);

// Setup database provider
builder.Services.AddDbContext<DataContext>(options =>
{
    //todo: using Sqlite for testing. Change to sql server later
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), 
        b => b.MigrationsAssembly("Prodaja karata za gradski prijevoz"));
});

string spaUrl = builder.Configuration["SPA:Url"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "SPA", builder =>
    {
        // todo: add to config
        builder.WithOrigins(spaUrl)
            .SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var emailConfig = builder.Configuration.GetSection("Email:Server").Get<EmailConfiguration>();

// singleton services
builder.Services.TryAddSingleton(emailConfig);
builder.Services.TryAddSingleton<IObjectMapperService, ObjectMapperService>();
builder.Services.TryAddSingleton<IHashingService, HashingService>();

// transient services
builder.Services.TryAddTransient<IEmailService, EmailService>();
builder.Services.TryAddTransient<IAuthService, AuthService>();

// Repositories registration
builder.Services.TryAddScoped<IKorisnikRepozitorij, KorisnikRepository>();
builder.Services.TryAddScoped<IRegistracijskiTokenRepository, RegistracijskiTokenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SPA");
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
app.Run();
