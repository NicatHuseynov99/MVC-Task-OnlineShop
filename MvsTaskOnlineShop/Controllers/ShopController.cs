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
            int pageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / take);
            int count = 1 + ((page - 1) * take);
            Pagination<Product> model = new Pagination<Product>(products, page, pageCount, count);
            return View(model);
        }
    }
}
