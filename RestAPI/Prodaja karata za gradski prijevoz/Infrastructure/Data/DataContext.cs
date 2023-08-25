using Domain.Entities.Tickets;
using Domain.Entities.Payment;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Invoices;
using Domain.Entities.Requests;
using Domain.Entities.Reviews;
using Domain.Entities.News;

namespace Infrastructure.Data;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RegistrationToken> RegistrationTokens { get; set; } = null!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;
    public DbSet<PaymentMethod> PaymentOptions { get; set; } = null!;
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Status> Statuses { get; set; } = null!;
    public DbSet<IssuedTicket> IssuedTickets { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<News> News { get; set; } = null!;
    public DbSet<Tax> Taxes { get; set; } = null!;


    public DataContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
           .HasIndex(u => u.Email)
           .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne<Role>()
            .WithMany()
            .HasForeignKey(r => r.RoleId);

        modelBuilder.Entity<User>()
            .HasOne<Status>()
            .WithMany()
            .HasForeignKey(user => user.UserStatusId);

        modelBuilder.Entity<Request>()
            .HasOne<Status>()
            .WithMany()
            .HasForeignKey(r => r.UserStatusId);

        modelBuilder.Entity<Status>()
            .Property(p => p.Discount)
            .HasPrecision(5, 2);

        modelBuilder.Entity<Tax>()
            .Property(t => t.Percentage)
            .HasPrecision(5, 2);

        BuildPaymentOptions(modelBuilder);
        BuildUserRoles(modelBuilder);
        BuildUserStatus(modelBuilder);
        BuildTicketData(modelBuilder);
        BuildTaxesData(modelBuilder);
    }

    private static void BuildTaxesData(ModelBuilder modelBuilder)
    {
        Tax tax = new()
        {
            Id = new Guid("e363863b-ba6d-477f-9afb-15dcbf70616b"),
            Name = "PDV",
            Percentage = 0.17,
            Active = true
        };

        modelBuilder.Entity<Tax>()
            .HasData(tax);
    }

    private static void BuildPaymentOptions(ModelBuilder modelBuilder)
    {
        List<PaymentMethod> paymentOptions = new()
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

        modelBuilder.Entity<PaymentMethod>()
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
                Discount=0.3

            },
            new()
            {
                Id=new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"),
                Name="Pensioner",
                Discount=0.5
            },
            new()
            {
                Id=new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"),
                Name="Employed",
                Discount=0.15
            },
            new()
            {
                Id=new Guid("9647c387-b0fb-4336-9434-079249f37e76"),
                Name="Unemployed",
                Discount=0.4
            }
        };

        modelBuilder.Entity<Status>()
            .HasData(statuses);
    }

    private static void BuildTicketData(ModelBuilder modelBuilder)
    {
        List<Ticket> tickets = new()
        {
            new()
            {
                Id=new Guid("929cb30e-ae11-4653-8f20-41c3b39102bd"),
                Name="Jednosmjerna",
                Active=true,
                Price=1.80
            },
            new()
            {
                Id=new Guid("5dd86adc-50be-4db6-98c5-46c4a582b61a"),
                Name="Povratna",
                Active=true,
                Price=3.20
            },
            new()
            {
                Id=new Guid("b8eec999-55ff-47b5-9ce0-cfedcabadba6"),
                Name="Dnevna",
                Active=true,
                Price=7.10
            },
            new()
            {
                Id=new Guid("fb272ac2-6c72-40fc-a425-96da10a0077c"),
                Name="Dječija",
                Active=true,
                Price=0.60
            }
        };
        modelBuilder.Entity<Ticket>()
             .HasData(tickets);
    }

}
