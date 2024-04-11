using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.Helpers;
using MvsTaskOnlineShop.Utilities.Paginate;

namespace MvsTaskOnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1, int take = 5)
        {
            int count = 1 + (page - 1) * take;
            List<Product> products = _context.Products
                .Include(m => m.Category)
                .Include(m => m.Sizes)
                .Include(m => m.Colors)
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = (int)Math.Ceiling((decimal)_context.Products.Count() / take);
            Pagination<Product> result = new Pagination<Product>(products, page, pageCount, count);
            return View(result);
        }
        public IActionResult Read(int id)
        {
            Product? product = _context.Products.Include(m => m.Category).Include(m => m.Sizes).Include(m => m.Colors).FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/categories", product.Image);
            Helper.DeleteFile(path);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Featured = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Active(int id)
        {
            Product? product = await _context.Products.Include(m=>m.Category).FirstOrDefaultAsync(m=>m.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            if (product.Category == null)
            {
                return NotFound();
            }
            if (product.Category.IsActive == true)
            {
                product.Featured = true;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
