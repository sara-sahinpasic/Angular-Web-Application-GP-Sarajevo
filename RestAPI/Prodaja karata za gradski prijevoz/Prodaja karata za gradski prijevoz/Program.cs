using Application.Abstractions.Email;
using Domain.Abstractions.Interfaces.Korisnici;
using Infrastructure.Data;
using Infrastructure.Repositories.Korisnici;
using Infrastructure.Services.Email;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Setup database provider
builder.Services.AddDbContext<DataContext>(options =>
{
    //todo: using Sqlite for testing. Change to sql server later
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"), 
        b => b.MigrationsAssembly("Prodaja karata za gradski prijevoz"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var emailConfig = builder.Configuration.GetSection("Email:Mailjet").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddTransient<IEmailHandler, EmailHandler>();

// Repositories registration
builder.Services.AddScoped<IKorisnikRepozitorij, KorisnikRepository>();
builder.Services.AddScoped<IRegistracijskiTokenRepository, RegistracijskiTokenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
