using System.ComponentModel.DataAnnotations;

namespace Playground.Models;

public class Country : BaseEntity
{
    [Required(ErrorMessage = "Укажите страну.")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов.")]
    public string Name { get; set; } = string.Empty;

    public ICollection<Product> Products { get; set; } = new List<Product>();
}
