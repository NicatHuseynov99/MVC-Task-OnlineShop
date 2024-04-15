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
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Color> model = _context.Colors.ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Color model)
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
            Color? Color = await _context.Colors.FindAsync(id);
            if (Color == null)
            {
                return NotFound();
            }
            _context.Colors.Remove(Color);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Color? Color = _context.Colors.Find(id);
            if (Color == null)
            {
                return NotFound();
            }
            return View(Color);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Color model)
        {
            Color? Color = await _context.Colors.FindAsync(id);
            if (Color == null)
            {
                return NotFound();
            }
            if (model.Name == null)
            {
                ModelState.AddModelError("Name", "Name is required");
                return View(Color);
            }
            Color.Name = model.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
