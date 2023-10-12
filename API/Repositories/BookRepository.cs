using API.Data;
using API.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System;
using Microsoft.EntityFrameworkCore;
using API.DTO;

namespace API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Book>> GetBooks()
        { 
           var books = await _context.Books
                .Include(b=>b.Authors)
                .ToListAsync();
           return books;
        
        }

        public async Task<Book> GetBookByID(int id) 
        {
            try
            {
                return await _context.Books
                        .Include(b => b.Authors)
                        .FirstAsync(book => book.Id == id);
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Book> AddBook(Book book) {

            if (book == null)
            {
               throw new ArgumentNullException(nameof(book));
            }

             await _context.Books.AddAsync(book);
            return book;
        }

    
       public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }


    }
}
