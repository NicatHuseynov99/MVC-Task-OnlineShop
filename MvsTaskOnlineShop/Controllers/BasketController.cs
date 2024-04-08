using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.ViewModels;
using Newtonsoft.Json;
namespace MvsTaskOnlineShop.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        public BasketController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<BasketDetailVM> products = new List<BasketDetailVM>();
            if (Request.Cookies["basket"] == null)
            {
                return View(products);
            }

            List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            foreach (var item in basket)
            {
                Product product = _context.Products.FirstOrDefault(m => m.Id == item.Id);
                BasketDetailVM basketProduct = new BasketDetailVM()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Count = item.Count,
                    SizeName=item.SizeName,
                    ColorName=item.ColorName,
                };
                products.Add(basketProduct);
            }
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddBasket(int id, int? sizeId, int? colorId, int productCount = 1)
        {
            Product? product = await _context.Products.Include(x=>x.Sizes).Include(x=>x.Colors).FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            await UpdateBasket(id, sizeId, colorId, productCount,product);

            return RedirectToAction("Index");
        }
        public Task UpdateBasket(int id, int? sizeId, int? colorId, int productCount,Product product)
        {
            List<BasketVM> basket = GetBasket();
            if (sizeId == null)
            {
                sizeId=product.Sizes.FirstOrDefault().Id;
            }
            if (colorId == null)
            {
                colorId = product.Colors.FirstOrDefault().Id;
            }
            string exitsSizeName = product.Sizes.FirstOrDefault(m => m.Id == sizeId).Name;
            string exitsColorName = product.Colors.FirstOrDefault(m => m.Id == colorId).Name;
            BasketVM exitsProduct = basket.FirstOrDefault(m => m.Id == id && m.SizeName==exitsSizeName && m.ColorName==exitsColorName);
            if (exitsProduct == null)
            {
                basket.Add(new BasketVM
                {
                    Id = id,
                    Count = productCount,
                    SizeName = exitsSizeName,
                    ColorName = exitsColorName,
                });
            }
            else
            {
                exitsProduct.Count += productCount;
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return Task.CompletedTask;
        }

        public List<BasketVM> GetBasket()
        {
            if (Request.Cookies.ContainsKey("basket"))
            {
                return JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            return new List<BasketVM>();
        }

        [HttpPost]
        public IActionResult RemoveBasket(int id)
        {
            List<BasketVM> basket = GetBasket();
            if (basket.Count > 0)
            {
                basket.Remove(basket.FirstOrDefault(m => m.Id == id));
                Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            }
            if (basket.Count == 0)
            {
                Response.Cookies.Delete("basket");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ReduceBasket(int id)
        {
            List<BasketVM> basket = GetBasket();
            BasketVM exitsProduct = basket.FirstOrDefault(m => m.Id == id);
            if (exitsProduct == null)
            {
                return NotFound();
            }
            if (exitsProduct.Count > 1)
            {
                exitsProduct.Count--;
            }
            else
            {
                basket.Remove(exitsProduct);
            }
            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return RedirectToAction("Index");
        }
    }
}
