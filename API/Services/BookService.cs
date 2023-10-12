using API.DTO;
using API.Entities;
using API.Repositories;
using AutoMapper;

namespace API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IAuthorService _authorService;

        public BookService(IBookRepository bookRepository, IMapper mapper, IAuthorService authorService)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorService = authorService;
        }
        public async Task<ICollection<BookDTO>> GetBooks() {
            var books = await _bookRepository.GetBooks();

            var bookDTOs = _mapper.Map<List<BookDTO>>(books); 
            
            return bookDTOs;
        
        }
        public async Task<BookDTO> GetBookByID(int id) {
            Book book = await _bookRepository.GetBookByID(id);
            var bookDTO = _mapper.Map<BookDTO>(book);
            return bookDTO;
        }
       public async Task<Book> AddBook(BookDTO bookDTO) {
            try
            {
                Book book = _mapper.Map<Book>(bookDTO);

                book.Authors = await _authorService.AddAuthors(bookDTO.Authors);
                await _bookRepository.AddBook(book);
                await _bookRepository.SaveChanges();
                return book;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            }

    }
}
