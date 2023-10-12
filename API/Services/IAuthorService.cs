using API.DTO;
using API.Entities;

namespace API.Services
{
    public interface IAuthorService
    {
        Task<ICollection<Author>> AddAuthors(ICollection<AuthorDTO> authorDTO);
    }
}
