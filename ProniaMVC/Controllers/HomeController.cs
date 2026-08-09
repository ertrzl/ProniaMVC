using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index() 
        {
            List<Slide> slides = await _context.Slides
                .OrderBy(s => s.Order)
                .Take(2)
                .ToListAsync();

            List<Product> products = await _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null))
                .ToListAsync();

            HomeVM homeVM = new HomeVM()
            {
                Slides = slides,
                Products = products
            };
            return View(homeVM);
        }
    }
}
