using API.Data;
using API.DTO;
using API.DTO.Filters;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace API.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext _context;

        public BookRepository(DataContext context)
        {
            _context = context;
        }
       
        public async Task<ICollection<Book>> GetBooks(PageRequest pageRequest)
        {
            var queryBooks = _context.Books.AsQueryable();

            if (!pageRequest.Title.Equals(null))
                queryBooks = queryBooks.Where(b => b.Title.Contains(pageRequest.Title));

            if (pageRequest.Author != null)
                queryBooks = queryBooks.Where
                    (b => b.Authors.Any(a => a.NameOfAuthor.Contains(pageRequest.Author)));

            // Apply sorting for sort by 
            if (!pageRequest.SortByTitle.ToString().Equals(null))
                queryBooks = pageRequest.SortByTitle.Equals("ASC") ? 
                    queryBooks.OrderBy(b => b.Title) : 
                    queryBooks.OrderByDescending(b => b.Title);

            if (!pageRequest.SortByAuthor.Equals(null))
                queryBooks = pageRequest.SortByAuthor.Equals("ASC") ?
                    queryBooks.OrderBy(b => b.Authors.FirstOrDefault().NameOfAuthor) : 
                    queryBooks.OrderByDescending(b => b.Authors.FirstOrDefault().NameOfAuthor);

            // Apply pagination
            queryBooks = queryBooks.Skip((pageRequest.StartPage - 1) * pageRequest.LimitPage).
                Take(pageRequest.LimitPage);

            var book = await queryBooks.ToListAsync();
              
            return book;
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
        public async Task<Book> AddBook(Book book)
        {

            if (book == null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            await _context.Books.AddAsync(book);
            return book;
        }
        public async Task<bool> DeleteBookByID(int id)
        {
            try
            {
                var book = await GetBookByID(id);
                _context.Remove(book);
                await _context.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool UpdateBookByID(Book book)
        {
            
            try
            {
                _context.Update(book);
                 return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                //handle concurency conflicts(HTTP 409)
                return false;
            }
            catch (DbUpdateException)
            {
                //handle other update errors(422)
                return false;
            }
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
