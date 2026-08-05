using Microsoft.AspNetCore.Mvc;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;

        public HomeController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index()
        {
            List<Slide> slides = _context.Slides
                .OrderBy(s => s.Order)
                .Take(2)
                .ToList();



            HomeVM homeVM = new HomeVM()
            {
                Slides = slides
                
            };
            return View(homeVM);
        }
    }
}
