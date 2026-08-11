using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories
                .Where(c => c.IsDeleted == false)
                .ToListAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View(category);

            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), "A category with this name already exists.");
                return View(category);
            }

            category.CreatedAt = DateTime.Now;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Category category)
        {
            if (id is null || id < 1 || category is null) return BadRequest();
            if (!ModelState.IsValid) return View(category);

            bool result = await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(Category.Name), "A category with this name already exists.");
                return View(category);
            }

            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            existed.Name = category.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();

            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Category? category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category is null) return NotFound();

            return View(category);
        }
    }
}
