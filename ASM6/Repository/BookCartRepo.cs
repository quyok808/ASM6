using ASM6.Models;

namespace ASM6.Repository
{
    public class BookCartRepo : IBookCart
    {
        List<Book> Carts;

        public BookCartRepo()
        {
            Carts = new List<Book>();
        }

        public void Add(Book book)
        {
            Carts.Add(book);
        }

        public void Delete(int id)
        {
            Book? find = GetById(id);
            if (find != null)
            {
                Carts.Remove(find);
            }
        }

        public List<Book> getAll()
        {
            return Carts;
        }

        public Book? GetById(int id)
        {
            return Carts.FirstOrDefault(p => p.ID == id);
        }
    }
}
