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
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;
        public SizeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Size> model = _context.Sizes.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Size model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Size? size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Size? size =  _context.Sizes.Find(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Size model)
        {
            Size? size = await _context.Sizes.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            if (model.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(size);
            }
            size.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
