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
            HomeVM model = new ()
            {
                Categories = categories,
                Products = products,
            };
            return View(model);
        }
    }
}
