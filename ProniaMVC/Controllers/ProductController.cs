using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaMVC.DAL;
using ProniaMVC.Models;
using ProniaMVC.ViewModels;

namespace ProniaMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            if (id is null|| id < 1) return BadRequest();

            Product? product = _context.Products
                .OrderByDescending(p => p.Id)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == id);

            if (product is null) return NotFound();

            List<Product> relatedProducts = _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToList();

            DetailVM detailVM = new()
            {
                Product = product,
                RelatedProducts = relatedProducts
            };



            return View(detailVM);
        }
    }
}
