using Microsoft.EntityFrameworkCore;
using Playground.Models;

namespace Playground.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        await db.Database.MigrateAsync();

        if (await db.Manufacturers.AnyAsync())
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;

        var samsung = new Manufacturer
        {
            Name = "Samsung",
            Description = "Производитель электроники",
            CreatedAt = now,
            UpdatedAt = now
        };

        var apple = new Manufacturer
        {
            Name = "Apple",
            Description = "Производитель потребительской электроники",
            CreatedAt = now,
            UpdatedAt = now
        };

        var phone = new EquipmentType
        {
            Name = "Телефон",
            Description = "Мобильные телефоны и смартфоны",
            CreatedAt = now,
            UpdatedAt = now
        };

        var tablet = new EquipmentType
        {
            Name = "Планшет",
            Description = "Планшетные компьютеры",
            CreatedAt = now,
            UpdatedAt = now
        };

        var china = new Country
        {
            Name = "Китай",
            CreatedAt = now,
            UpdatedAt = now
        };

        var vietnam = new Country
        {
            Name = "Вьетнам",
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Manufacturers.AddRange(samsung, apple);
        db.EquipmentTypes.AddRange(phone, tablet);
        db.Countries.AddRange(china, vietnam);
        await db.SaveChangesAsync();

        db.Products.AddRange(
            new Product
            {
                Model = "Galaxy S24",
                Price = 79990m,
                ManufacturerId = samsung.Id,
                EquipmentTypeId = phone.Id,
                CountryId = vietnam.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Model = "iPhone 15",
                Price = 99990m,
                ManufacturerId = apple.Id,
                EquipmentTypeId = phone.Id,
                CountryId = china.Id,
                CreatedAt = now,
                UpdatedAt = now
            },
            new Product
            {
                Model = "Galaxy Tab S9",
                Price = 84990m,
                ManufacturerId = samsung.Id,
                EquipmentTypeId = tablet.Id,
                CountryId = vietnam.Id,
                CreatedAt = now,
                UpdatedAt = now
            });

        await db.SaveChangesAsync();
    }
}
