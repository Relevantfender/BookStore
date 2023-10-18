using API.DTO;
using API.Entities;

namespace API.Services
{
    public interface IAuthorService
    {
        Task<ICollection<Author>> AddAuthors(ICollection<AuthorDTO> authorDTO);
        Task<bool> DeleteAuthorFromBook(int bookid, AuthorDTO authorDTO);
        Task<IList<Author>> GetExistingAuthors(ICollection<AuthorDTO> authorDTOs);
    }
}
