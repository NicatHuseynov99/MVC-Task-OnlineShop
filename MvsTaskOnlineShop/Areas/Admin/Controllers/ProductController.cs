using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.Paginate;

namespace MvsTaskOnlineShop.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        [Area("Admin")]
        public IActionResult Index(int page = 1, int take = 5)
        {
            int count = 1 + (page - 1) * take;
            List<Product> products = _context.Products
                .Include(m => m.Category)
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / take);
            Pagination<Product> result = new Pagination<Product>(products, page, pageCount, count);
            return View(result);
        }
    }
}
