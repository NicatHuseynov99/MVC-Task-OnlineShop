using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.Paginate;

namespace MvsTaskOnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1, int take = 4)
        {
            int number = 1 + (page - 1) * take;
            List<Category> categories = _context.Categories
                .OrderByDescending(m => m.Id)
            .Skip((page - 1) * take)
            .Take(take)
                .ToList(); ;
            int pageCount = (int)Math.Ceiling((decimal)_context.Categories.Count() / take);
            int count = 1 + ((page - 1) * take);

            Pagination<Category> result = new Pagination<Category>(categories, page, pageCount, count);
            return View(result);
        }
    }
}
