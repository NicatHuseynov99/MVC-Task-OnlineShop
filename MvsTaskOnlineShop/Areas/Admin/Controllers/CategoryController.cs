using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.File;
using MvsTaskOnlineShop.Utilities.Helpers;
using MvsTaskOnlineShop.Utilities.Paginate;

namespace MvsTaskOnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!model.Photo.CheckFileType("image/"))
            {
                return View(model);
            }
            if (!model.Photo.CheckFileSize(3500))
            {
                ModelState.AddModelError("Photo", "File size is wrong");
                return View(model);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
            string path = Helper.GetFilePath(_env.WebRootPath, "img/categories", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await model.Photo.CopyToAsync(stream);
            }
            model.Image = fileName;
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/categories", category.Image);
            Helper.DeleteFile(path);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Active(int id)
        {
            Category? category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.IsActive == true)
            {
                return RedirectToAction("Index");
            }
            category.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            Category? category = await _context.Categories.Include(m => m.Products).FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            if (category.IsActive == false)
            {
                return RedirectToAction("Index");
            }
            category.IsActive = false;
            foreach (var item in category.Products)
            {
                item.Featured = false;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category model)
        {
            Category? category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            if (model.Photo != null)
            {
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is wrong");
                    return View(category);
                }
                if (!model.Photo.CheckFileSize(3000))
                {
                    ModelState.AddModelError("Photo", "File size is wrong");
                    return View(category);
                }
                string path = Helper.GetFilePath(_env.WebRootPath, "img/categories", category.Image);
                Helper.DeleteFile(path);
                string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string newPath = Helper.GetFilePath(_env.WebRootPath, "img/categories", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                category.Image = fileName;
            }

            if (model.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(category);
            }
            category.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
