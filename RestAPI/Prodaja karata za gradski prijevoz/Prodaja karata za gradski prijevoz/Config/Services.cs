using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Implementations.Auth;
using Application.Services.Implementations.Hashing;
using Application.Services.Implementations.Mapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Invoices;
using Infrastructure.Repositories.Requests;
using Infrastructure.Repositories.Users;
using Infrastructure.Services.Email;
using Infrastructure.Services.File;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static partial class Services
{
    public static void AddConfig(this WebApplicationBuilder builder)
    {
        EmailConfiguration emailConfig = builder.Configuration.GetSection("Email:Server").Get<EmailConfiguration>();
        AuthConfirmationConfig authConfig = builder.Configuration.GetSection("AuthConfig").Get<AuthConfirmationConfig>();

        builder.Services.TryAddSingleton(emailConfig);
        builder.Services.TryAddSingleton(authConfig);
    }

    public static void AddSingletonServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<IObjectMapperService, ObjectMapperService>();
        builder.Services.TryAddSingleton<IPasswordService, PasswordService>();
    }

    public static void AddTransientServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddTransient<IEmailService, EmailService>();
        builder.Services.TryAddTransient<IAuthService, AuthService>();
        builder.Services.TryAddTransient<IFileService, FileService>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUserRepository, UserRepository>();
        builder.Services.TryAddScoped<IRegistrationTokenRepository, RegistrationTokenRepository>();
        builder.Services.TryAddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        builder.Services.TryAddScoped<IUserStatusRepository, UserStatusRepository>();
        builder.Services.TryAddScoped<IRequestRepository, RequestRepository>();
        builder.Services.TryAddScoped<IInvoiceRepository, InvoiceRepository>();
    }

    public static void AddScopedServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
    }
}
