using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM6.Data;
using ASM6.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace ASM6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class BooksController : Controller
    {
        private readonly BookDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BooksController(BookDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/Books
        public async Task<IActionResult> Index()
        {
            var bookDbContext = _context.Books.Include(b => b.Category);
            return View(await bookDbContext.ToListAsync());
        }

        // GET: Admin/Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Admin/Books/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile FileUpLoad)
        {
            ModelState.Remove("FileUpLoad");
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Admin/Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // POST: Admin/Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Book book, IFormFile FileUpLoad)
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
                        var filePath = Path.Combine(UploadFolder, book.ID + ".jpg");

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await FileUpLoad.CopyToAsync(fileStream);
                        }
                    }
                    book.Image = book.ID + ".jpg";
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // GET: Admin/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Admin/Books/Delete/5
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
