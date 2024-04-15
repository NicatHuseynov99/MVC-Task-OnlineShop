using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvsTaskOnlineShop.Models
{
    public class Partner
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string? Image {  get; set; }
        public bool IsActive { get; set; }
        [NotMapped]
        [Required]
        public IFormFile Photo { get; set; }
    }
}
