using Application.Config.Email;
using Application.Config.SMS;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Abstractions.Interfaces.SMS;
using Application.Services.Implementations.Auth;
using Application.Services.Implementations.Hashing;
using Application.Services.Implementations.Mapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Users;
using Infrastructure.Services.Email;
using Infrastructure.Services.SMS;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static partial class Services
{
    public static void AddConfig(this WebApplicationBuilder builder)
    {
        var emailConfig = builder.Configuration.GetSection("Email:Server").Get<EmailConfiguration>();
        var smsConfig = builder.Configuration.GetSection("SMS").Get<SMSConfig>();

        builder.Services.TryAddSingleton(emailConfig);
        builder.Services.TryAddSingleton(smsConfig);
    }

    public static void AddSingletonServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddSingleton<IObjectMapperService, ObjectMapperService>();
        builder.Services.TryAddSingleton<IPasswordService, PasswordService>();
    }

    public static void AddTransientServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddTransient<IEmailService, EmailService>();
        builder.Services.TryAddTransient<ISMSService, SmsService>();
        builder.Services.TryAddTransient<IAuthService, AuthService>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUserRepository, UserRepository>();
        builder.Services.TryAddScoped<IRegistrationTokenRepository, RegistrationTokenRepository>();
        builder.Services.TryAddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
    }

    public static void AddScopedServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
    }
}
