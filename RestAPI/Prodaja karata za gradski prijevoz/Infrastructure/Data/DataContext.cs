using Domain.Entities.Korisnici;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public sealed class DataContext : DbContext
{
    public DbSet<Korisnik> Users { get; set; }
    public DbSet<RegistracijskiToken> RegistrationTokens { get; set; }
    public DbSet<VerificationCode> VerificationCodes { get; set; }


    public DataContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Korisnik>()
           .HasIndex(u => u.Email)
           .IsUnique();
    }
}
