using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvsTaskOnlineShop.Models
{
    public class Carousel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
        public bool IsActive { get; set; }
    }
}
