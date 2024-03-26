using ASM6.Models;

namespace ASM6.Repository
{
    public interface IBookCart
    {
        List<Book> getAll();
        void Add(Book book);
        void Delete(int id);
        Book? GetById(int id);
    }
}
