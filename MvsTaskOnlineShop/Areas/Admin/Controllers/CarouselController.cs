using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvsTaskOnlineShop.Data;
using MvsTaskOnlineShop.Models;
using MvsTaskOnlineShop.Utilities.File;
using MvsTaskOnlineShop.Utilities.Helpers;

namespace MvsTaskOnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CarouselController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public CarouselController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Carousel> model = _context.Carousels.ToList();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Carousel model)
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
            string path = Helper.GetFilePath(_env.WebRootPath, "img/carousels", fileName);
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
            Carousel? carousel = await _context.Carousels.FindAsync(id);
            if (carousel == null)
            {
                return NotFound();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/carousels", carousel.Image);
            Helper.DeleteFile(path);
            _context.Carousels.Remove(carousel);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Active(int id)
        {
            Carousel? carousel =  _context.Carousels.Find(id);
            if (carousel == null)
            {
                return NotFound();
            }
            if (carousel.IsActive == true)
            {
                return RedirectToAction("Index");
            }
            carousel.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            Carousel? carousel = _context.Carousels.Find(id);
            if (carousel == null)
            {
                return NotFound();
            }
            if (carousel.IsActive == false)
            {
                return RedirectToAction("Index");
            }
            carousel.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Carousel? carousel = _context.Carousels.Find(id);
            if (carousel == null)
            {
                return NotFound();
            }
            return View(carousel);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Carousel model)
        {
            Carousel? carousel = _context.Carousels.Find(id);
            if (carousel == null)
            {
                return NotFound();
            }
            if (model.Photo != null)
            {
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is wrong");
                    return View(carousel);
                }
                if (!model.Photo.CheckFileSize(3000))
                {
                    ModelState.AddModelError("Photo", "File size is wrong");
                    return View(carousel);
                }
                string path = Helper.GetFilePath(_env.WebRootPath, "img/carousels", carousel.Image);
                Helper.DeleteFile(path);
                string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string newPath = Helper.GetFilePath(_env.WebRootPath, "img/carousels", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                carousel.Image = fileName;
            }

            if (model.Title == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(carousel);
            }
            if (model.Description == null)
            {
                ModelState.AddModelError("Name", "Description is required");
                return View(carousel);
            }
            carousel.Title = model.Title;
            carousel.Description = model.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
