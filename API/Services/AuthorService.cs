using API.DTO;
using API.Entities;
using API.Repositories;
using AutoMapper;

namespace API.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;
        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<Author>> AddAuthors(ICollection<AuthorDTO> authorDTO)
        {
            ICollection<Author> authors = _mapper.Map<ICollection<Author>>(authorDTO);
            
            await _authorRepository.AddAuthors(authors);
            await _authorRepository.SaveChanges();
            return authors;
           
        }
      

       
    }
}
