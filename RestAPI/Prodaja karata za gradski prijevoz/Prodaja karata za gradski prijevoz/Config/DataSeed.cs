using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Application.Services.Abstractions.Interfaces.Repositories.Invoices;
using Application.Services.Abstractions.Interfaces.Repositories.News;
using Application.Services.Abstractions.Interfaces.Repositories.Payment;
using Application.Services.Abstractions.Interfaces.Repositories.Reviews;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Application.Services.Abstractions.Interfaces.Repositories.Stations;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Driver;
using Domain.Entities.Invoices;
using Domain.Entities.News;
using Domain.Entities.Payment;
using Domain.Entities.Reviews;
using Domain.Entities.Stations;
using Domain.Entities.Tickets;
using Domain.Entities.Users;
using Domain.Entities.Vehicles;
using Domain.Enums.PaymentOption;
using Domain.Enums.Ticket;
using Domain.Enums.User;
using Route = Domain.Entities.Routes.Route;

namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static class DataSeed
{
    private const int UserCount = 5;
    private const string DefaultUserPassword = "0000";
    private const int InvoicesCount = 100;
    private const int NewsCount = 20;
    private const int ReviewsCount = 15;
    private const int StationsCount = 15;
    private const int VehiclesCount = 10;
    private const int RoutesCount = 100;
    private const string AdminEmail = "sara.sahinpasic@hotmail.com";
    private const string DriverEmail = "driver@mail.com";
    private const int DelaysCount = 5;
    private const int MalfunctionCount = 3;


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

        await SeedNews(userIds, unitOfWork, serviceProvider);
        await SeedReviews(unitOfWork, serviceProvider);

        IEnumerable<Station> stations = await SeedStations(unitOfWork, serviceProvider);
        IEnumerable<Manufacturer> manufacturers = await SeedManufacturers(unitOfWork, serviceProvider);
        IEnumerable<VehicleType> vehicleTypes = await SeedVehicleTypes(unitOfWork, serviceProvider);
        IEnumerable<Vehicle> vehicles = await SeedVehicles(vehicleTypes, manufacturers, unitOfWork, serviceProvider);

        IEnumerable<Guid> routeIds = await SeedRoutes(stations, vehicles, unitOfWork, serviceProvider);
        ICollection<IssuedTicket> issuedTickets = await SeedIssuedTickets(unitOfWork, userIds, routeIds, serviceProvider);
        await SeedInvoices(unitOfWork, issuedTickets, serviceProvider);

        ICollection<Delay> delays = await SeedDelays(unitOfWork, routeIds, serviceProvider);
        ICollection<Malfunction> malfunctions = await SeedMalfunction(unitOfWork, serviceProvider, vehicles);

    }
    private static async Task<ICollection<Malfunction>> SeedMalfunction(IUnitOfWork unitOfWork,
       IServiceProvider serviceProvider, IEnumerable<Vehicle> vehicles)
    {
        IMalfunctionRepository malfunctionRepository = serviceProvider.GetRequiredService<IMalfunctionRepository>();
        ICollection<Malfunction> malfunctions = new List<Malfunction>();

        for (int i = 0; i < MalfunctionCount; ++i)
        {
            Malfunction malfunction = new()
            {
                Description = Faker.Lorem.Sentence(4),
                DateOfMalufunction = DateTime.Now,
                Fixed = new Random().NextDouble() >= 0.5,
                Vehicle = vehicles.ElementAt(Random.Shared.Next(0, vehicles.Count())),
            };

            malfunctionRepository.Create(malfunction);
            malfunctions.Add(malfunction);
        }
        await unitOfWork.CommitAsync(default);
        return malfunctions;
    }
    private static async Task<ICollection<Delay>> SeedDelays(IUnitOfWork unitOfWork,
    IEnumerable<Guid> routeIds, IServiceProvider serviceProvider)
    {
        IDelayRepository delayRepository = serviceProvider.GetRequiredService<IDelayRepository>();

        ICollection<Delay> delays = new List<Delay>();

        for (int i = 0; i < DelaysCount; ++i)
        {
            Delay delay = new()
            {
                DelayAmount = new Random().Next(1, 60),
                Reason = Faker.Lorem.Sentence(4),
                RouteId = routeIds.ElementAt(Random.Shared.Next(0, routeIds.Count())),
            };

            delayRepository.Create(delay);
            delays.Add(delay);
        }
        await unitOfWork.CommitAsync(default);
        return delays;
    }
    private static async Task SeedReviews(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IReviewRepository reviewRepository = serviceProvider.GetRequiredService<IReviewRepository>();
        for (int i = 0; i < ReviewsCount; i++)
        {
            Review review = new Review
            {
                Title = Faker.Company.Name(),
                Description = Faker.Lorem.Paragraph(),
                Score = new Random().Next(1, 5)
            };
            reviewRepository.Create(review);
        }
        await unitOfWork.CommitAsync(default);
    }
    private static async Task SeedNews(IEnumerable<Guid> userIds, IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        INewsRepository newsRepository = serviceProvider.GetRequiredService<INewsRepository>();
        for (int i = 0; i < NewsCount; i++)
        {
            News news = new News
            {
                Title = Faker.Company.Name(),
                Content = Faker.Lorem.Paragraph(),
                Date = DateTime.UtcNow,
                UserId = userIds.First(),
            };
            newsRepository.Create(news);
        }
        await unitOfWork.CommitAsync(default);
    }

    private static async Task<ICollection<IssuedTicket>> SeedIssuedTickets(IUnitOfWork unitOfWork, ICollection<Guid> userIds, IEnumerable<Guid> routeIds, IServiceProvider serviceProvider)
    {
        IIssuedTicketRepository issuedTicketRepository = serviceProvider.GetRequiredService<IIssuedTicketRepository>();
        ITicketRepository ticketRepository = serviceProvider.GetRequiredService<ITicketRepository>();

        Ticket? oneWayTicket = await ticketRepository.GetByIdAsync(new Guid(Tickets.OneWay.ToString()), default);
        ICollection<IssuedTicket> issuedTickets = new List<IssuedTicket>();

        for (int i = 0; i < InvoicesCount; ++i)
        {
            int randomUserIndex = Random.Shared.Next(0, UserCount);

            IssuedTicket issuedTicket = new()
            {
                TicketId = new Guid(Tickets.OneWay.ToString()),
                Ticket = oneWayTicket,
                UserId = userIds.ElementAt(randomUserIndex),
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddMinutes(60),
                IssuedDate = DateTime.Now,
                RouteId = routeIds.ElementAt(Random.Shared.Next(0, routeIds.Count())),
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

    private static async Task<ICollection<Guid>> SeedUsers(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IUserRepository userRepository = serviceProvider.GetRequiredService<IUserRepository>();

        if (userRepository.GetAll().Any())
        {
            return new List<Guid>();
        }

        IPasswordService passwordService = serviceProvider.GetRequiredService<IPasswordService>();

        ICollection<Guid> userIds = new List<Guid>();

        for (int i = 0; i < UserCount; ++i)
        {
            string email = Faker.Internet.Email();
            Roles role = Roles.User;

            if (i == 0)
            {
                email = AdminEmail;
                role = Roles.Admin;
            }
            else if (i == 1)
            {
                email = DriverEmail;
                role = Roles.Driver;
            }

            User user = new()
            {
                Email = email,
                FirstName = Faker.Name.First(),
                LastName = Faker.Name.Last(),
                Active = true,
                DateOfBirth = Faker.Identification.DateOfBirth(),
                PhoneNumber = Faker.Phone.Number(),
                Address = Faker.Address.StreetAddress(),
                RegistrationDate = DateTime.UtcNow,
                Roles = role,
                Status = Statuses.Default,
                ProfileImagePath = ""
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

    public static async Task<IEnumerable<Station>> SeedStations(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IStationRepository stationRepository = serviceProvider.GetRequiredService<IStationRepository>();

        ICollection<Station> stationIds = new List<Station>();

        for (int i = 0; i < StationsCount; ++i)
        {
            Station station = new()
            {
                Name = Faker.Country.Name(),
            };

            stationRepository.Create(station);
            stationIds.Add(station);
        }
        await unitOfWork.CommitAsync(default);

        return stationIds;
    }

    public static async Task<IEnumerable<VehicleType>> SeedVehicleTypes(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IVehicleTypeRepository vehicleTypeRepository = serviceProvider.GetRequiredService<IVehicleTypeRepository>();

        ICollection<VehicleType> vehicleTypes = new List<VehicleType>();
        string[] types = new[] { "Bus", "Tram" };

        for (int i = 0; i < 2; ++i)
        {
            VehicleType vehicleType = new()
            {
                Name = types[i]
            };

            vehicleTypeRepository.Create(vehicleType);
            vehicleTypes.Add(vehicleType);
        }
        await unitOfWork.CommitAsync(default);

        return vehicleTypes;
    }

    public static async Task<IEnumerable<Vehicle>> SeedVehicles(IEnumerable<VehicleType> vehicleTypes, IEnumerable<Manufacturer> manufacturers, IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IVehicleRepository vehicleRepository = serviceProvider.GetRequiredService<IVehicleRepository>();

        ICollection<Vehicle> vehicles = new List<Vehicle>();
        string[] colors = new[] { "Blue", "Red", "Yellow", "Green", "Orange" };

        for (int i = 0; i < VehiclesCount; ++i)
        {
            Vehicle vehicle = new()
            {
                BuildYear = Random.Shared.Next(1995, 2023),
                Color = colors[Random.Shared.Next(0, colors.Length)],
                Manufacturer = manufacturers.ElementAt(Random.Shared.Next(0, manufacturers.Count())),
                VehicleType = vehicleTypes.ElementAt(Random.Shared.Next(0, vehicleTypes.Count())),
                Number = Random.Shared.Next(1, 30),
                RegistrationNumber = Faker.Identification.SocialSecurityNumber(),
            };

            vehicleRepository.Create(vehicle);
            vehicles.Add(vehicle);
        }

        await unitOfWork.CommitAsync(default);

        return vehicles;
    }

    public static async Task<IEnumerable<Manufacturer>> SeedManufacturers(IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IManufacturerRepository manufacturerRepository = serviceProvider.GetRequiredService<IManufacturerRepository>();

        ICollection<Manufacturer> manufacturers = new List<Manufacturer>();

        for (int i = 0; i < 2; ++i)
        {
            Manufacturer manufacturer = new()
            {
                Name = Faker.Name.First(),
            };

            manufacturerRepository.Create(manufacturer);
            manufacturers.Add(manufacturer);
        }
        await unitOfWork.CommitAsync(default);

        return manufacturers;
    }

    public static async Task<IEnumerable<Guid>> SeedRoutes(IEnumerable<Station> stations, IEnumerable<Vehicle> vehicles, IUnitOfWork unitOfWork, IServiceProvider serviceProvider)
    {
        IRouteRepository routeRepository = serviceProvider.GetRequiredService<IRouteRepository>();

        List<Guid> routeIds = new();

        for (int i = 0; i < RoutesCount; ++i)
        {
            TimeSpan timeOfDeparture = TimeSpan.FromHours(Random.Shared.Next(0, 24));
            Guid startStationId = stations.ElementAt(Random.Shared.Next(0, stations.Count())).Id;
            Route route = new()
            {
                Vehicle = vehicles.ElementAt(Random.Shared.Next(0, vehicles.Count())),
                TimeOfArival = timeOfDeparture.Add(TimeSpan.FromMinutes(30)),
                TimeOfDeparture = timeOfDeparture,
                Active = true,
                ActiveOnHolidays = true,
                ActiveOnWeekends = true,
                StartStationId = startStationId,
                EndStationId = stations.Where(station => station.Id != startStationId).ElementAt(Random.Shared.Next(0, stations.Count() - 1)).Id
            };

            routeRepository.Create(route);
            routeIds.Add(route.Id);
        }

        await unitOfWork.CommitAsync(default);

        return routeIds;
    }
}
