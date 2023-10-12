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

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >=0;
        }
    }
}
