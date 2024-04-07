using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.ViewModels;
using Newtonsoft.Json;

namespace MvsTaskOnlineShop.Controllers
{
    public class HeartController : Controller
    {
        private readonly AppDbContext _context;
        public HeartController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products = new List<Product>();
            if (Request.Cookies["heart"] == null)
            {
                return View(products);
            }
            List<HeartVM> heart = JsonConvert.DeserializeObject<List<HeartVM>>(Request.Cookies["heart"]);
            foreach (var item in heart)
            {
                Product product = _context.Products.FirstOrDefault(m => m.Id == item.Id);
                products.Add(product);
            }
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddHeart(int id)
        {
            Product? product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            await UpdateBasket(id);

            return RedirectToAction("Index");
        }

        public Task UpdateBasket(int id)
        {
            List<HeartVM> heart = GetHeart();
            HeartVM exitsProduct = heart.FirstOrDefault(m => m.Id == id);
            if (exitsProduct == null)
            {
                heart.Add(new HeartVM { Id = id, });
            }
            Response.Cookies.Append("heart", JsonConvert.SerializeObject(heart));

            return Task.CompletedTask;
        }

        public List<HeartVM> GetHeart()
        {
            if (Request.Cookies.ContainsKey("heart"))
            {
                return JsonConvert.DeserializeObject<List<HeartVM>>(Request.Cookies["heart"]);
            }
            return new List<HeartVM>();
        }
        [HttpPost]
        public IActionResult RemoveHeart(int id)
        {
            List<HeartVM> heart = GetHeart();
            if (heart.Count > 0)
            {
                heart.Remove(heart.FirstOrDefault(m => m.Id == id));
                Response.Cookies.Append("heart", JsonConvert.SerializeObject(heart));
            }
            if (heart.Count == 0)
            {
                Response.Cookies.Delete("heart");
            }
            return RedirectToAction("Index");
        }
    }
}
