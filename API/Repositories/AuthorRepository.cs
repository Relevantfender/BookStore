using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly DataContext _context;

        public AuthorRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Author>> GetAuthors()
        {
            return await _context.Authors.ToListAsync(); ;
        }
        public async Task<ICollection<Author>> AddAuthors(ICollection<Author> authors)
        {
            if (authors.Count == 0)
            {
                throw new ArgumentNullException(nameof(authors));
            }
            await _context.Authors.AddRangeAsync(authors);
            return authors;
        }
        public async Task<bool> DeleteAuthorFromBook(int bookId, Author author)
        {
            try
            {
                var book = await _context.Books
                    .Include(b => b.Authors)
                    .FirstOrDefaultAsync(b => b.Id == bookId);

                if (book == null)
                    return false;

                // Check if the author is associated with the book
                var bookAuthor = book.Authors.FirstOrDefault(a =>
                    string.Equals(a.NameOfAuthor.ToLower(), author.NameOfAuthor.ToLower()) &&
                    string.Equals(a.LastNameOfAuthor.ToLower(), author.LastNameOfAuthor.ToLower()) &&
                    a.DateOfBirth == author.DateOfBirth);

                if (bookAuthor != null)
                {
                    // Remove the author from the book's authors
                    book.Authors.Remove(bookAuthor);

                    // If this was the only book for the author, remove the author
                    if (bookAuthor.Books.Count == 0)
                        _context.Authors.Remove(bookAuthor);

                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<Author> GetAuthorByNameAndDateOfBirth(string nameOfAuthor, string lastNameOfAuthor, DateOnly dateOfBirth)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(a =>
                    a.NameOfAuthor == nameOfAuthor &&
                    a.LastNameOfAuthor == lastNameOfAuthor &&
                    a.DateOfBirth.Equals(dateOfBirth));
        }


        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >=0;
        }
    }
}
