using API.DTO;
using API.DTO.Filters;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Services
{
    public interface IBookService
    {
        Task<Book> AddBook(BookDTO bookDTO);
        List<BookDTO> GetBooks(PageRequest pageRequest);
        Task<BookDTO> GetBookByID(int id);
        Task<bool> DeleteBookByID(int id);
        Task<bool> UpdateBookByID(int id, BookDTO bookDTO);
    }
}
