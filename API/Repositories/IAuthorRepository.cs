using API.Entities;

namespace API.Repositories
{
    public interface IAuthorRepository
    {
        Task <ICollection<Author>> AddAuthors(ICollection<Author> authors);
        Task<ICollection<Author>> GetAuthors();
        Task<bool> DeleteAuthorFromBook(int bookId, Author author);
        Task<Author> GetAuthorByNameAndDateOfBirth(string nameOfAuthor, string lastNameOfAuthor, DateOnly dateOfBirth);
        Task<bool> SaveChanges();
    }
}
