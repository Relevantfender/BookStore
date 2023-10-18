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

        public async Task<List<Book>> GetBooks(PageRequest pageRequest)
        {
            var queryBooks = _context.Books.Include(b => b.Authors).
                AsQueryable();
            //filter by title 
            if (!string.IsNullOrEmpty(pageRequest.Title))

                queryBooks = queryBooks.Where(b => b.Title.ToLower().Contains(pageRequest.Title.ToLower()));
            //filter by author
            if (!string.IsNullOrEmpty(pageRequest.Author))
                queryBooks = queryBooks.Where
                    (b => b.Authors.Any(a => a.NameOfAuthor.ToLower().Contains(pageRequest.Author.ToLower())));

            PageRequest compare = new PageRequest
            {
                Author = null,
                Title = null,
                StartPage = 1,
                LimitPage = 30,
                SortByTitle = null,
                SortByAuthor = null
            };
            // Sort by ID if no parameters are entered


            if (pageRequest.Equals(compare))
            {
                // Sort by ID if no parameters are entered
                queryBooks = queryBooks.OrderBy(b => b.Id);
            }
            else
            {
                //sorting by asc or desc for title/author
                if (!pageRequest.SortByTitle.ToString().Equals(null))
                    queryBooks = pageRequest.SortByTitle.Equals("ASC") ?
                        queryBooks.OrderBy(b => b.Title) :
                        queryBooks.OrderByDescending(b => b.Title);

                if (!pageRequest.SortByAuthor.Equals(null))
                    queryBooks = pageRequest.SortByAuthor.Equals("ASC") ?
                        queryBooks.OrderBy(b => b.Authors.FirstOrDefault().NameOfAuthor) :
                        queryBooks.OrderByDescending(b => b.Authors.FirstOrDefault().NameOfAuthor);

            }
            // Apply pagination
            queryBooks = queryBooks.Skip((pageRequest.StartPage - 1) * pageRequest.LimitPage).
                Take(pageRequest.LimitPage);

            var book = await queryBooks.ToListAsync();

            return book;
        }
        public async Task<Book> GetBookByID(int id)
        {

            Book book = await _context.Books
                 .Include(b => b.Authors)
                 .FirstOrDefaultAsync(book => book.Id == id);

            if (book == null)
            {
                throw new NotFoundException(); // Throw a NotFoundException
            }

            return book;

        }
        public async Task<Book> AddBook(Book book)
        {
           
           
                await _context.Books.AddAsync(book);
                return book;
            
        }


        public async Task<Book> GetBookByTitleAndAuthors(string title, ICollection<AuthorDTO> authors)
        {
            var books = await _context.Books.Include(b => b.Authors).ToListAsync();

            var book = books.FirstOrDefault(b =>
                b.Title == title &&
                b.Authors.Any(a => authors.Any(aDTO =>
                    a.NameOfAuthor == aDTO.NameOfAuthor && a.LastNameOfAuthor == aDTO.LastNameOfAuthor)));
            if (book == default)
            {
                return null;
            }
            return book;

        }
        public async Task<bool> DeleteBookByID(int id)
        {
            
            try
            {
                var book = await GetBookByID(id);
                if (book == null)
                {
                    throw new NotFoundException(); // Throw a NotFoundException
                }
                foreach (var author in book.Authors)
                {
                    if (author.Books.Count == 1 && author.Books.Contains(book))
                    {
                        _context.Authors.Remove(author);
                    }
                }
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

            {

                try
                {
                    _context.Attach(book);
                    _context.Entry(book).State = EntityState.Modified;
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
        }

        //resets autoincrement value to 1 in database
        public void SQLCommand()
        {

            var SQLCommand = "BEGIN TRANSACTION; DELETE FROM Books; DELETE FROM Authors; DELETE FROM AuthorBook; UPDATE sqlite_sequence SET seq = 0; COMMIT TRANSACTION;";

            _context.Database.ExecuteSqlRaw(SQLCommand);

        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
