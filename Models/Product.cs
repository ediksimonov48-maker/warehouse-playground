using System.ComponentModel.DataAnnotations;

namespace Playground.Models;

public class Product : BaseEntity
{
    [Required(ErrorMessage = "Укажите модель.")]
    [StringLength(200, ErrorMessage = "Модель не должна превышать 200 символов.")]
    public string Model { get; set; } = string.Empty;

    public string? PhotoPath { get; set; }

    [Range(0.01, 1000000000000, ErrorMessage = "Цена должна быть больше нуля.")]
    public decimal Price { get; set; }

    public int EquipmentTypeId { get; set; }
    public EquipmentType EquipmentType { get; set; } = null!;

    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;
}
