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
        public IActionResult Index() 
        {
            List<Slide> slides = _context.Slides
                .OrderBy(s => s.Order)
                .Take(2)
                .ToList();

            List<Product> products = _context.Products
                .Include(p => p.ProductImages.Where(pi => pi.IsPrimary != null))
                .ToList();

            HomeVM homeVM = new HomeVM()
            {
                Slides = slides,
                Products = products
            };
            return View(homeVM);
        }
    }
}
