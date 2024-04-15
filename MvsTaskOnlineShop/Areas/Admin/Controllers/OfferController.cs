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
    public class OfferController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public OfferController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Offer> model = _context.Offers.ToList();
            if(model == null)
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
        public async Task<IActionResult> Create(Offer model)
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
            string path = Helper.GetFilePath(_env.WebRootPath, "img/Offers", fileName);
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
            Offer? offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/offers", offer.Image);
            Helper.DeleteFile(path);
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Active(int id)
        {
            Offer? offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            if (offer.IsActive == true)
            {
                return RedirectToAction("Index");
            }
            offer.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            Offer? offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            if (offer.IsActive == false)
            {
                return RedirectToAction("Index");
            }
            offer.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Offer? offer = _context.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Offer model)
        {
            Offer? offer = _context.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }
            if (model.Photo != null)
            {
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is wrong");
                    return View(offer);
                }
                if (!model.Photo.CheckFileSize(3000))
                {
                    ModelState.AddModelError("Photo", "File size is wrong");
                    return View(offer);
                }
                string path = Helper.GetFilePath(_env.WebRootPath, "img/offers", offer.Image);
                Helper.DeleteFile(path);
                string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string newPath = Helper.GetFilePath(_env.WebRootPath, "img/offers", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                offer.Image = fileName;
            }

            if (model.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(offer);
            }
            if (model.Description == null)
            {
                ModelState.AddModelError("Name", "Description is required");
                return View(offer);
            }
            offer.Name = model.Name;
            offer.Description = model.Description;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}