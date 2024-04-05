using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
namespace Ogani.ViewComponents
{
    public class NavCategoryViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public NavCategoryViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return await Task.FromResult(View(categories));
        }
    }
}
