using System.ComponentModel.DataAnnotations;

namespace Playground.Models;

public class EquipmentType : BaseEntity
{
    [Required(ErrorMessage = "Укажите тип техники.")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов.")]
    public string Description { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
