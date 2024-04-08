using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;

namespace MvsTaskOnlineShop.Controllers
{
    public class ShopDetailController : Controller
    {
        private readonly AppDbContext _context;
        public ShopDetailController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int id)
        {
            Product? product=_context.Products.Include(x=>x.Colors).Include(x=>x.Sizes).FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
