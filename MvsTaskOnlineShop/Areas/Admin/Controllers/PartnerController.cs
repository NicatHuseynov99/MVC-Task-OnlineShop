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
    [Authorize(Roles = "Admin")]
    public class PartnerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public PartnerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1, int take = 4)
        {
            int number = 1 + (page - 1) * take;
            List<Partner> partners = _context.Partners
            .OrderByDescending(m => m.Id)
            .Skip((page - 1) * take)
            .Take(take)
            .ToList();
            int pageCount = (int)Math.Ceiling((decimal)_context.Partners.Count() / take);
            int count = 1 + ((page - 1) * take);
            Pagination<Partner> result = new Pagination<Partner>(partners, page, pageCount, count);
            return View(result);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Partner model)
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
            string path = Helper.GetFilePath(_env.WebRootPath, "img/partners", fileName);
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
            Partner? partner = await _context.Partners.FindAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            string path = Helper.GetFilePath(_env.WebRootPath, "img/partners", partner.Image);
            Helper.DeleteFile(path);
            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Active(int id)
        {
            Partner? partner = await _context.Partners.FindAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            if (partner.IsActive == true)
            {
                return RedirectToAction("Index");
            }
            partner.IsActive = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Disable(int id)
        {
            Partner? partner = await _context.Partners.FindAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            if (partner.IsActive == false)
            {
                return RedirectToAction("Index");
            }
            partner.IsActive = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Partner? partner =  _context.Partners.Find(id);
            if (partner == null)
            {
                return NotFound();
            }
            return View(partner);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Partner model)
        {
            Partner? partner = _context.Partners.Find(id);
            if (partner == null)
            {
                return NotFound();
            }
            if (model.Photo != null)
            {
                if (!model.Photo.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Photo", "File type is wrong");
                    return View(partner);
                }
                if (!model.Photo.CheckFileSize(3000))
                {
                    ModelState.AddModelError("Photo", "File size is wrong");
                    return View(partner);
                }
                string path = Helper.GetFilePath(_env.WebRootPath, "img/partners", partner.Image);
                Helper.DeleteFile(path);
                string fileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string newPath = Helper.GetFilePath(_env.WebRootPath, "img/partners", fileName);
                using (FileStream stream = new FileStream(newPath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }
                partner.Image = fileName;
            }

            if (model.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(partner);
            }
            partner.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
