using MvsTaskOnlineShop.Models;

namespace MvsTaskOnlineShop.ViewModels
{
    public class HomeVM
    {
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Offer> Offers { get; set; }
        public List<Carousel> Carousels { get; set; }
    }
}
