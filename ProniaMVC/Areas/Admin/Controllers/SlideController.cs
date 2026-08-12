using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.Utilities.Enums;
using ProniaMVC.Utilities.Extensions;

namespace ProniaMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext appDbContext, IWebHostEnvironment env)
        {
            _context = appDbContext;
            _env = env;

            _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.Where(s => s.IsDeleted == false).ToListAsync();

            return View(slides);
        }
        public async Task<IActionResult> Archive()
        {
            List<Slide> slides = await _context.Slides.Where(s => s.IsDeleted==true).ToListAsync();

            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(slide);
            //}

            if (!slide.Photo.IsValidSize(FileSize.MB, 2)) 
            {
                ModelState.AddModelError(nameof(Slide.Photo), "Image size must be less than 2 MB.");
                return View(slide);
            }
            if (!slide.Photo.IsValidType("image"))
            {
                ModelState.AddModelError(nameof(Slide.Photo), "Image format is not allowed.");
                return View(slide);
            }
            bool result = await _context.Slides.AnyAsync(s => s.Order == slide.Order);

            if (result)
            {
                ModelState.AddModelError(nameof(Slide.Order), "A slide with this order already exists.");
                return View(slide);
            }        


            

            slide.Image = await slide.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images", "website-images");
            slide.CreatedAt = DateTime.Now;

            _context.Slides.Add(slide);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            return View(existed);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, Slide slide)
        {
            if(id is null || id < 1 || slide is null) return BadRequest();
            if(!ModelState.IsValid) return View(slide);

            bool result = await _context.Slides.AnyAsync(s => s.Order == slide.Order && s.Id != id);
            if (result)
            {
                ModelState.AddModelError(nameof(Slide.Order), "A slide with this order already exists.");
                return View(slide);
            }
            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id); 
            if (existed is null) return NotFound(); 

            existed.Title = slide.Title;
            existed.SubTitle = slide.SubTitle;
            existed.Order = slide.Order;
            existed.Image = slide.Image;
            existed.Description = slide.Description;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));        

        }



        public async Task<IActionResult>Delete(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);

            if (existed is null) return NotFound();

            _context.Slides.Remove(existed);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id < 1) return BadRequest();

            Slide? slide = await _context.Slides
                .FirstOrDefaultAsync(s => s.Id == id);

            if (slide is null) return NotFound();

            return View(slide);
        }

    }
}
