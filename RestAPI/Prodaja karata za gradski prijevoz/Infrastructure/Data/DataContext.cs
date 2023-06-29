using Domain.Entities.Tickets;
using Domain.Entities.Payment;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Invoices;
using Domain.Entities.Requests;

namespace Infrastructure.Data;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RegistrationToken> RegistrationTokens { get; set; } = null!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<PaymentOption> PaymentOptions { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Request> Requests { get; set; } = null!;
    public DbSet<RequestType> RequestTypes { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;

    public DataContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne<Role>()
            .WithOne()
            .HasForeignKey<User>(r => r.RoleId);

        modelBuilder.Entity<Request>()
            .HasOne<RequestType>()
            .WithOne()
            .HasForeignKey<Request>(r => r.RequestTypeId);

        BuildPaymentOptions(modelBuilder);
        BuildUserRoles(modelBuilder);
        BuildRequestTypes(modelBuilder);
        BuildUserStatus(modelBuilder);
    }

    private static void BuildPaymentOptions(ModelBuilder modelBuilder)
    {
        List<PaymentOption> paymentOptions = new()
        {
            new()
            {
                Id = new Guid("8e5264f5-0eea-4fae-9945-80d835583ba1"),
                Name = "Card"
            },
            new()
            {
                Id = new Guid("46536a11-f5b3-4505-a13a-e7d44dda9ae9"),
                Name = "Mail"
            }
        };

        modelBuilder.Entity<PaymentOption>()
            .HasData(paymentOptions);
    }

    private static void BuildUserRoles(ModelBuilder modelBuilder)
    {
        List<Role> roles = new()
        {
            new()
            {
                Id = new Guid("f3bc7265-e8dc-4a3c-b04c-f7a881bcd939"),
                Name = "Admin"
            },
            new()
            {
                Id = new Guid("f3206708-33aa-4be0-b4ae-cb6cc10005cf"),
                Name = "User"
            },
            new()
            {
                Id = new Guid("f9fefebe-9bec-480f-bbec-431f72b14995"),
                Name = "Driver"
            }
        };

        modelBuilder.Entity<Role>()
            .HasData(roles);
    }

    private static void BuildRequestTypes(ModelBuilder modelBuilder)
    {
        List<RequestType> requestTypes = new()
        {
            new()
            {
                Id = new Guid("23a43e2c-ea65-4a33-9d5c-1195dfb72d43"),
                Name="Student"
            },
            new()
            {
                Id=new Guid("41a37a8d-4d5f-4353-988b-89cc2f7cb3db"),
                Name="Employed",
            },
            new()
            {
                Id=new Guid("6309f61b-4a1d-4866-befb-ffef76f8b869"),
                Name="Pensioner"
            },
            new()
            {
                Id=new Guid("6b989bc6-7314-4a5c-adca-f7b44ab3158a"),
                Name="Unemployed"
            }
        };

        modelBuilder.Entity<RequestType>()
            .HasData(requestTypes);
    }

    private static void BuildUserStatus(ModelBuilder modelBuilder)
    {
        List<Status> statuses = new()
        {
            new()
            {
                Id=new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"),
                Name = "Default",
                Discount=0
            },
            new()
            {
                Id=new Guid("056b4a11-96b3-413c-a323-0cef9a5680c2"),
                Name = "Student",
                Discount=30

            },
            new()
            {
                Id=new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"),
                Name="Pensioner",
                Discount=50
            },
            new()
            {
                Id=new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"),
                Name="Employed",
                Discount=15
            },
            new()
            {
                Id=new Guid("9647c387-b0fb-4336-9434-079249f37e76"),
                Name="Unemployed",
                Discount=40
            }
        };

        modelBuilder.Entity<Status>()
            .HasData(statuses);
    }

}
