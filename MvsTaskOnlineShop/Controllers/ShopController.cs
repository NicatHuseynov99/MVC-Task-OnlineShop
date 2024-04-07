using Microsoft.AspNetCore.Mvc;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.Paginate;

namespace MvsTaskOnlineShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        public ShopController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int take = 6)
        {
            List<Product> products = _context.Products
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int count = (int)Math.Ceiling((decimal)_context.Products.Count() / take);
            Pagination<Product> model = new Pagination<Product>(products, page, count);
            return View(model);
        }
    }
}
