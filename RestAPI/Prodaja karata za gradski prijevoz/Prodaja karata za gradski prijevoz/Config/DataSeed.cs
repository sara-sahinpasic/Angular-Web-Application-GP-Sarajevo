using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Invoices;
using Domain.Entities.Payment;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Enums.PaymentOption;
using Domain.Enums.Ticket;
using Domain.Enums.User;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static class DataSeed
{
    private const int UserCount = 5;
    private const string DefaultUserPassword = "0000";
    private const int InvoicesCount = 100;

    public static async Task SeedData(this WebApplication app)
    {
        using IServiceScope serviceScope = app.Services.CreateScope();
        IServiceProvider serviceProvider = serviceScope.ServiceProvider;

        IUnitOfWork unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        
        ICollection<Guid> userIds = await SeedUsers(unitOfWork, serviceProvider);

        if (userIds.Count == 0)
        {
            return;
        }

        ICollection<IssuedTicket> issuedTickets =  await SeedIssuedTickets(unitOfWork, userIds, serviceProvider);

        await SeedInvoices(unitOfWork, issuedTickets, serviceProvider);
    }

    private static async Task<ICollection<IssuedTicket>> SeedIssuedTickets(IUnitOfWork unitOfWork, ICollection<Guid> userIds, IServiceProvider serviceProvider)
    {
        IIssuedTicketRepository issuedTicketRepository = serviceProvider.GetRequiredService<IIssuedTicketRepository>();
        ITicketRepository ticketRepository = serviceProvider.GetRequiredService<ITicketRepository>();

        Ticket? oneWayTicket = await ticketRepository.GetByIdAsync(new Guid(Tickets.OneWay.ToString()), default);
        ICollection<IssuedTicket> issuedTickets = new List<IssuedTicket>(); ;

        for (int i = 0; i < InvoicesCount; ++i)
        {
            int randomUserIndex = Random.Shared.Next(0, UserCount);

            IssuedTicket issuedTicket= new()
            {
                Id = Guid.NewGuid(),
                TicketId = new Guid(Tickets.OneWay.ToString()),
                Ticket = oneWayTicket,
                UserId = userIds.ElementAt(randomUserIndex),
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddMinutes(60),
                IssuedDate = DateTime.Now,
            };

            issuedTicketRepository.Create(issuedTicket);
            issuedTickets.Add(issuedTicket);
        }
        await unitOfWork.CommitAsync(default);

        return issuedTickets;
    }

    private static async Task SeedInvoices(IUnitOfWork unitOfWork, ICollection<IssuedTicket> issuedTickets, IServiceProvider serviceProvider)
    {
        IInvoiceRepository invoiceRepository = serviceProvider.GetRequiredService<IInvoiceRepository>();
        ITaxRepository taxRepository = serviceProvider.GetRequiredService<ITaxRepository>();

        issuedTickets = issuedTickets.OrderBy(issuedTicket => issuedTicket.UserId).ToList();
        Tax? tax = await taxRepository.GetActiveAsync(default);

        while (issuedTickets.Count > 0) 
        {
            int ticketPerInvoice = Random.Shared.Next(1, 6);
            IssuedTicket? issuedTicket = issuedTickets.FirstOrDefault();

            if (issuedTicket is null) 
            {
                break;
            }

            double total = issuedTickets.Take(ticketPerInvoice).Sum(t => t.Ticket.Price);
            double totalWithoutTaxes = total - (total * tax.Percentage);

            Invoice invoice = new()
            {
                Id = Guid.NewGuid(),
                PaymentOption = PaymentOptions.Card,
                Total = 1.8 * ticketPerInvoice,
                InvoicingDate = DateTime.Now,
                UserId = issuedTicket.UserId,
                IssuedTickets = issuedTickets.Where(it => it.UserId == issuedTicket.UserId).Take(ticketPerInvoice).ToList(),
                TotalWithoutTax = totalWithoutTaxes,
                Tax = tax
            };

            issuedTickets = issuedTickets.Skip(ticketPerInvoice).ToList();

            invoiceRepository.Create(invoice);
        }

        await unitOfWork.CommitAsync(default);
    }

    private static async Task<ICollection<Guid>> SeedUsers(IUnitOfWork unitOfWork, IServiceProvider app)
    {
        IUserRepository userRepository = app.GetRequiredService<IUserRepository>();
        
        if (userRepository.GetAll().Any())
        {
            return new List<Guid>();
        }
        
        IPasswordService passwordService = app.GetRequiredService<IPasswordService>();

        ICollection<Guid> userIds = new List<Guid>();

        for (int i = 0; i < UserCount; ++i)
        {
            User user = new()
            {
                Email = Faker.Internet.Email(),
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                Active = true,
                DateOfBirth = Faker.Identification.DateOfBirth(),
                Id = Guid.NewGuid(),
                PhoneNumber = Faker.Phone.Number(),
                Address = Faker.Address.StreetAddress(),
                RegistrationDate = DateTime.UtcNow,
                Role = Roles.User,
                Status = Statuses.Default,
                ProfileImagePath=""
            };
            Tuple<byte[], string> passwordHashAndSalt = passwordService.GeneratePasswordHashAndSalt(DefaultUserPassword);

            user.PasswordHash = passwordHashAndSalt.Item2;
            user.PasswordSalt = passwordHashAndSalt.Item1;
            userRepository.Create(user);
            userIds.Add(user.Id);
        }
        await unitOfWork.CommitAsync(default);

        return userIds;
    }
}
