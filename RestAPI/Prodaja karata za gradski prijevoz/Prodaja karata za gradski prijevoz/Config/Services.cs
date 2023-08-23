using Application.Config.Email;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Checkout;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Application.Services.Abstractions.Interfaces.Repositories.Reviews;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Implementations.Auth;
using Application.Services.Implementations.Checkout;
using Application.Services.Implementations.Hashing;
using Application.Services.Implementations.Mapper;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Invoices;
using Infrastructure.Repositories.Payment;
using Infrastructure.Repositories.Requests;
using Infrastructure.Repositories.Reviews;
using Infrastructure.Repositories.Tickets;
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
    }

    public static void AddTransientServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddTransient<IEmailService, EmailService>();
        builder.Services.TryAddTransient<IAuthService, AuthService>();
        builder.Services.TryAddTransient<IFileService, FileService>();
        builder.Services.TryAddTransient<ICheckoutService, CheckoutService>();
        builder.Services.TryAddTransient<IObjectMapperService, ObjectMapperService>();
        builder.Services.TryAddTransient<IPasswordService, PasswordService>();
        builder.Services.TryAddTransient<IPDFGeneratorService, PDFGeneratorService>();
    }

    public static void AddRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUserRepository, UserRepository>();
        builder.Services.TryAddScoped<IRegistrationTokenRepository, RegistrationTokenRepository>();
        builder.Services.TryAddScoped<IVerificationCodeRepository, VerificationCodeRepository>();
        builder.Services.TryAddScoped<IUserStatusRepository, UserStatusRepository>();
        builder.Services.TryAddScoped<IRequestRepository, RequestRepository>();
        builder.Services.TryAddScoped<IInvoiceRepository, InvoiceRepository>();
        builder.Services.TryAddScoped<IIssuedTicketRepository, IssuedTicketRepository>();
        builder.Services.TryAddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        builder.Services.TryAddScoped<ITicketRepository, TicketRepository>();
        builder.Services.TryAddScoped<IReviewRepository, ReviewRepository>();
        builder.Services.TryAddScoped<ITaxRepository, TaxRepository>();
    }

    public static void AddScopedServices(this WebApplicationBuilder builder)
    {
        builder.Services.TryAddScoped<IUnitOfWork, UnitOfWork>();
    }
}
