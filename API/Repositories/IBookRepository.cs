using API.Entities;

namespace API.Repositories
{
    public interface IBookRepository
    {
        Task<Book> AddBook(Book book);
        Task<ICollection<Book>> GetBooks();
        Task<bool> SaveChanges();
        Task<Book> GetBookByID(int id);
    }
}
