using Microsoft.EntityFrameworkCore;
using Playground.Data;
using Playground.Models;

namespace Playground.Services;

public sealed record ProductPageData(
    List<Product> Products,
    List<Manufacturer> Manufacturers,
    List<EquipmentType> EquipmentTypes,
    List<Country> Countries);

public class ProductService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<ProductPageData> GetPageDataAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var products = await db.Products
            .Include(x => x.Manufacturer)
            .Include(x => x.EquipmentType)
            .Include(x => x.Country)
            .AsNoTracking()
            .OrderBy(x => x.Model)
            .ToListAsync();

        var manufacturers = await db.Manufacturers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        var equipmentTypes = await db.EquipmentTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        var countries = await db.Countries
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new ProductPageData(products, manufacturers, equipmentTypes, countries);
    }

    public async Task SaveAsync(Product form)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var now = DateTimeOffset.UtcNow;

        if (form.Id == 0)
        {
            db.Products.Add(new Product
            {
                Model = form.Model.Trim(),
                PhotoPath = form.PhotoPath?.Trim(),
                Price = form.Price,
                ManufacturerId = form.ManufacturerId,
                EquipmentTypeId = form.EquipmentTypeId,
                CountryId = form.CountryId,
                CreatedAt = now,
                UpdatedAt = now
            });
        }
        else
        {
            var product = await db.Products.FindAsync(form.Id)
                ?? throw new InvalidOperationException("Товар не найден.");

            product.Model = form.Model.Trim();
            product.PhotoPath = form.PhotoPath?.Trim();
            product.Price = form.Price;
            product.ManufacturerId = form.ManufacturerId;
            product.EquipmentTypeId = form.EquipmentTypeId;
            product.CountryId = form.CountryId;
            product.UpdatedAt = now;
        }

        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var product = await db.Products.FindAsync(id);

        if (product is null)
        {
            return false;
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<Country> AddCountryAsync(string name)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var normalizedName = name.Trim();

        var existingCountry = await db.Countries
            .FirstOrDefaultAsync(x => x.Name == normalizedName);

        if (existingCountry is not null)
        {
            return existingCountry;
        }

        var now = DateTimeOffset.UtcNow;
        var country = new Country
        {
            Name = normalizedName,
            CreatedAt = now,
            UpdatedAt = now
        };

        db.Countries.Add(country);
        await db.SaveChangesAsync();
        return country;
    }
}
