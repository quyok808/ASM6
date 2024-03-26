using ASM6.Data;
using ASM6.Models;
using ASM6.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM6.Controllers
{
    [Authorize(Roles ="Admin, Member")]
    public class CartController : Controller
    {
        private readonly BookCartRepo _bookCartRepo;
        private readonly BookDbContext _context;

        public CartController(BookCartRepo bookCartRepo, BookDbContext context)
        {
            _bookCartRepo = bookCartRepo;
            _context = context;
        }

        public IActionResult Index()
        {
            TempData["ListCategory"] = _context.Categories.ToListAsync();
            TempData["ListBook"] = _context.Books.ToListAsync();
            return View(_bookCartRepo.getAll());
        }

        [HttpPost] // Đánh dấu action chỉ được gọi khi nhận yêu cầu HTTP POST
        public IActionResult AddToCart(int bookId)
        {

            // Tìm sách trong cơ sở dữ liệu dựa vào bookId
            var book = _context.Books.FirstOrDefault(b => b.ID == bookId);

            if (book == null)
            {
                return NotFound(); // Trả về lỗi 404 nếu không tìm thấy sách
            }
            TempData["ListCategory"] = _context.Categories.ToListAsync();
            _bookCartRepo.Add(book);

            return View("Index", _bookCartRepo.getAll()); // Trả về kết quả thành công
        }

        // GET: Book/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            Book? book = _bookCartRepo.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(Book item)
        {
            _bookCartRepo.Delete(item.ID);
            return View("Index", _bookCartRepo.getAll());
        }
    }
}