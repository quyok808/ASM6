using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM6.Models;
using ASM6.Data;
using SQLitePCL;

namespace ASM6.Controllers
{
    public class BookController : Controller
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BookController(BookDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Book
        public async Task<IActionResult> Index()
        {
            return View(await _context.Books.ToListAsync());
        }

        public async Task<IActionResult> Index_Category(int id)
        {
            TempData["ListCategory"] = await _context.Categories.ToListAsync();
            TempData["ListBook"] = await _context.Books.ToListAsync();
            List<Book> books = _context.Books.Where(p => p.CategoryId == id).ToList();
            return View(books);
        }

        // GET: Book/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            TempData["ListCategory"] = await _context.Categories.ToListAsync();
            TempData["ListBook"] = await _context.Books.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Book/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile FileUpLoad)
        {
            ModelState.Remove("FileUpLoad");
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                int bookid;
                if (_context.Books.Any())
                {
                    bookid = _context.Books.Max(p => p.ID) + 1;
                }
                else
                {
                    // Nếu không có bản ghi nào trong bảng Books, bạn có thể gán bookid bằng 1 hoặc giá trị mặc định của bạn.
                    bookid = 1;
                }
                book.ID = bookid;
                if (FileUpLoad != null && FileUpLoad.Length > 0)
                {
                    var UploadFolder = Path.Combine(_environment.WebRootPath, "imgs");
                    var filePath = Path.Combine(UploadFolder, book.ID + ".jpg");

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileUpLoad.CopyToAsync(fileStream);
                    }
                }
                book.Image = book.ID + ".jpg";
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book, IFormFile FileUpLoad)
        {
            if (id != book.ID)
            {
                return NotFound();
            }
            ModelState.Remove("FileUpLoad");
            if (ModelState.IsValid)
            {
                try
                {
                    if (FileUpLoad != null && FileUpLoad.Length > 0)
                    {
                        var UploadFolder = Path.Combine(_environment.WebRootPath, "imgs");
                        var filePath = Path.Combine(UploadFolder, id + ".jpg");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FileUpLoad.CopyToAsync(fileStream);
                        }
                    }
                    book.Image = id + ".jpg";

                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }
    }
}
