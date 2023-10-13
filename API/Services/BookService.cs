using API.DTO;
using API.DTO.Filters;
using API.Entities;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<List<BookDTO>> GetBooks(PageRequest pageRequest)
            {
            var books = await _bookRepository.GetBooks(pageRequest);
            return _mapper.Map<List<BookDTO>>(books);
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
        public async Task<bool> DeleteBookByID(int id)
        {
            return await _bookRepository.DeleteBookByID(id);

        }

        public async Task<bool> UpdateBookByID(int id, BookDTO bookDTO)
        {
            var book = await _bookRepository.GetBookByID(id);
            if (book == null)
                return false;

            _mapper.Map(bookDTO, book);
            //mapping it over from dto to book sets the id to 0 since it's not present in bookdto
            //so id is set to its value
            book.Id = id;
            _bookRepository.UpdateBookByID(book);
            
            await _bookRepository.SaveChanges();
            return true;

        }

    }
}

