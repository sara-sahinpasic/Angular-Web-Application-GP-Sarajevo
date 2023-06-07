using Domain.Entities.Payment;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class DataContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RegistrationToken> RegistrationTokens { get; set; } = null!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<PaymentOption> PaymentOptions { get; set; } = null!;

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

        BuildPaymentOptions(modelBuilder);
        modelBuilder.Entity<Role>()
            .HasData(roles);
    }

    private void BuildPaymentOptions(ModelBuilder modelBuilder)
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
}
