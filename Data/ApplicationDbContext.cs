using Microsoft.EntityFrameworkCore;
using Playground.Models;

namespace Playground.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();
    public DbSet<EquipmentType> EquipmentTypes => Set<EquipmentType>();
    public DbSet<Country> Countries => Set<Country>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<EquipmentType>(entity =>
        {
            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasIndex(x => x.Name)
                .IsUnique();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(x => x.Model)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            entity.HasOne(x => x.Manufacturer)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.EquipmentType)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.EquipmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Country)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
