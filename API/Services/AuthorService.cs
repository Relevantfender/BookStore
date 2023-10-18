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
        public async Task<bool> DeleteAuthorFromBook(int bookid,AuthorDTO authorDTO) 
        {
            var author = _mapper.Map<Author>(authorDTO);
            var operation = await _authorRepository.DeleteAuthorFromBook(bookid, author);
            if (operation)
            {
                return true;
            }
            else return false;
            

            
        }
        public async Task<IList<Author>> GetExistingAuthors(ICollection<AuthorDTO> authorDTOs)
        {
            List<Author> existingAuthors = new List<Author>();

            foreach (var authorDTO in authorDTOs)
            {
                Author existingAuthor = await _authorRepository
                    .GetAuthorByNameAndDateOfBirth(authorDTO.NameOfAuthor, authorDTO.LastNameOfAuthor, authorDTO.DateOfBirth);

                if (existingAuthor != null)
                {
                    existingAuthors.Add(existingAuthor);
                }
            }

            return existingAuthors;
        }



    }
}
