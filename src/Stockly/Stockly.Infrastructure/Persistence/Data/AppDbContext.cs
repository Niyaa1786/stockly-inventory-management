using Microsoft.EntityFrameworkCore;
using Stockly.Domain.Entities;
using Stockly.Domain.Enums;

namespace Stockly.Infrastructure.Persistence.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Role).IsRequired().HasConversion<string>().HasMaxLength(20);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasIndex(e => e.Email).IsUnique();
        });

        SeedData(modelBuilder);
    }
    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Role = Role.Admin,
            FullName = "Admin",
            Email = "admin@stockly.com",
            PasswordHash = "$2a$11$E3RGRhjfkGzTz5J42JIOXe3dpiCEGaiZZxLIYfm0qdwnc/xFU/w.u",
            IsActive = true,
            CreatedAt = new DateTime(2026, 11, 2),
        });
    }
}
