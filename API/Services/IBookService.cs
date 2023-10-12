using API.DTO;
using API.Entities;

namespace API.Services
{
    public interface IBookService
    {
        Task<Book> AddBook(BookDTO bookDTO);
        Task<ICollection<BookDTO>> GetBooks();
        Task<BookDTO> GetBookByID(int id);
    }
}
