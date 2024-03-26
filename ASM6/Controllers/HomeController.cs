using ASM6.Data;
using ASM6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASM6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookDbContext _context;

        public HomeController(ILogger<HomeController> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
			TempData["ListCategory"] = await _context.Categories.ToListAsync();
			TempData["ListBook"] = await _context.Books.ToListAsync();
			return View(await _context.Books.ToListAsync());
        }

		public async Task<IActionResult> Index_category(int id)
		{
			TempData["ListCategory"] = await _context.Categories.ToListAsync();
			TempData["ListBook"] = await _context.Books.ToListAsync();
			List<Book>? books = await _context.Books.Where(p => p.CategoryId == id).ToListAsync();
			return View(books);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
