using Microsoft.AspNetCore.Mvc;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.ViewModels;

namespace MvsTaskOnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            List<Product> products = _context.Products.ToList();
            List<Partner> partners= _context.Partners.ToList();
            List<Carousel> carousels = _context.Carousels.ToList();
            List<Offer> offers = _context.Offers.ToList();
            HomeVM model = new ()
            {
                Categories = categories,
                Products = products,
                Partners = partners,
                Offers = offers,
                Carousels = carousels
            };
            return View(model);
        }
    }
}
