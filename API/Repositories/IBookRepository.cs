﻿using API.DTO;
using API.DTO.Filters;
using API.Entities;

namespace API.Repositories
{
    public interface IBookRepository
    {
        Task<Book> AddBook(Book book);
        Task<List<Book>> GetBooks(PageRequest pageRequest);
        Task<Book> GetBookByTitleAndAuthors(string title, ICollection<AuthorDTO> authors);
        Task<bool> SaveChanges();
        Task<Book> GetBookByID(int id);
        Task<bool> DeleteBookByID(int id);
        bool UpdateBookByID(Book book);
        void SQLCommand();
    }
}
