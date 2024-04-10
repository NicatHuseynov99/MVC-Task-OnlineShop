using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvsTaskOnlineShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Image { get; set; }
        public List<Product>? Products { get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }

    }
}
