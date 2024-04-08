using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
namespace Ogani.ViewComponents
{
    public class ShopDetailViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        public ShopDetailViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return await Task.FromResult(View(products));
        }
    }
}
