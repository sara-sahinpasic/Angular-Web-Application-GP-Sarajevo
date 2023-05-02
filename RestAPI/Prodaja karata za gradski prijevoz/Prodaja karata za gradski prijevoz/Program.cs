using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Prodaja_karata_za_gradski_prijevoz.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddApplicationPart(Presentation.PresentationAssembly.Assembly);

// Setup database provider
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), 
        b => b.MigrationsAssembly("Prodaja karata za gradski prijevoz"));
});

string spaUrl = builder.Configuration["SPA:Url"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "SPA", builder =>
    {
        // todo: add to config: sprint 2
        builder.WithOrigins(spaUrl)
            .SetIsOriginAllowed(isOriginAllowed: _ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAuthenticationAndAuthorization();

builder.AddConfig();

builder.AddSingletonServices();
builder.AddRepositories();
builder.AddScopedServices();
builder.AddTransientServices();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
