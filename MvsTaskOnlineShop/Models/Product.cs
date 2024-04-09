using System.ComponentModel.DataAnnotations.Schema;

namespace MvsTaskOnlineShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public Category Category { get; set; }
        public List<Size> Sizes { get; set; }
        public List<Color> Colors { get; set; }
        public bool Featured { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
