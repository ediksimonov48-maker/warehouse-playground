using Microsoft.EntityFrameworkCore;
using Playground.Data;
using Playground.Models;

namespace Playground.Services;

public sealed record DirectoryData(
    List<Manufacturer> Manufacturers,
    List<EquipmentType> EquipmentTypes);

public class DirectoryService(IDbContextFactory<ApplicationDbContext> dbFactory)
{
    public async Task<DirectoryData> GetAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var manufacturers = await db.Manufacturers
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        var equipmentTypes = await db.EquipmentTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new DirectoryData(manufacturers, equipmentTypes);
    }

    public async Task SaveManufacturerAsync(Manufacturer form)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var now = DateTimeOffset.UtcNow;

        if (form.Id == 0)
        {
            db.Manufacturers.Add(new Manufacturer
            {
                Name = form.Name.Trim(),
                Description = form.Description.Trim(),
                CreatedAt = now,
                UpdatedAt = now
            });
        }
        else
        {
            var manufacturer = await db.Manufacturers.FindAsync(form.Id)
                ?? throw new InvalidOperationException("Производитель не найден.");

            manufacturer.Name = form.Name.Trim();
            manufacturer.Description = form.Description.Trim();
            manufacturer.UpdatedAt = now;
        }

        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteManufacturerAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        if (await db.Products.AnyAsync(x => x.ManufacturerId == id))
        {
            return false;
        }

        var manufacturer = await db.Manufacturers.FindAsync(id);
        if (manufacturer is null)
        {
            return false;
        }

        db.Manufacturers.Remove(manufacturer);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task SaveEquipmentTypeAsync(EquipmentType form)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var now = DateTimeOffset.UtcNow;

        if (form.Id == 0)
        {
            db.EquipmentTypes.Add(new EquipmentType
            {
                Name = form.Name.Trim(),
                Description = form.Description.Trim(),
                CreatedAt = now,
                UpdatedAt = now
            });
        }
        else
        {
            var equipmentType = await db.EquipmentTypes.FindAsync(form.Id)
                ?? throw new InvalidOperationException("Тип техники не найден.");

            equipmentType.Name = form.Name.Trim();
            equipmentType.Description = form.Description.Trim();
            equipmentType.UpdatedAt = now;
        }

        await db.SaveChangesAsync();
    }

    public async Task<bool> DeleteEquipmentTypeAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        if (await db.Products.AnyAsync(x => x.EquipmentTypeId == id))
        {
            return false;
        }

        var equipmentType = await db.EquipmentTypes.FindAsync(id);
        if (equipmentType is null)
        {
            return false;
        }

        db.EquipmentTypes.Remove(equipmentType);
        await db.SaveChangesAsync();
        return true;
    }
}
