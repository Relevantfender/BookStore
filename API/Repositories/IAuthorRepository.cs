using API.Entities;

namespace API.Repositories
{
    public interface IAuthorRepository
    {
        Task <ICollection<Author>> AddAuthors(ICollection<Author> authors);
        Task<ICollection<Author>> GetAuthors();
        Task<bool> SaveChanges();
    }
}
