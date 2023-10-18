using API.DTO;
using API.DTO.Filters;
using API.Entities;
using API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        public async Task<BookDTO> GetBookByID(int id)
        {

            try
            {
                Book book = await _bookRepository.GetBookByID(id);
                var bookDTO = _mapper.Map<BookDTO>(book);
                return bookDTO;
            }
            catch (NotFoundException)
            {
                throw; // Re-throw NotFoundException
            }

        }
        public async Task<Book> AddBook(BookDTO bookDTO)
        {
           
                // Check if the book already exists 
                Book existingBook = await _bookRepository.GetBookByTitleAndAuthors(bookDTO.Title, bookDTO.Authors);

                // If exists, return null so that controller can handle the conflict
                if (existingBook != null)
                {
                   return null;
                }
                //maps a dto->book
                Book book = _mapper.Map<Book>(bookDTO);
            

                await _bookRepository.AddBook(book);
                await _bookRepository.SaveChanges();

                return book;
            }
            
        }


        public async Task<bool> DeleteBookByID(int id)
        {
            try
            {
                return await _bookRepository.DeleteBookByID(id);
            }
            catch (NotFoundException)
            {
                throw; // Re-throw NotFoundException
            }


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



        public async Task<bool> ResetBooks()
        {
            //delete the table author, book, bookauthor and reset autoincrement as transaction

            _bookRepository.SQLCommand();

            //deserialize the json file into a list of books
            string json = File.ReadAllText("Data\\ResetDatabase.json");
            List<BookDTO> StandardLibrary = JsonConvert.DeserializeObject<List<BookDTO>>(json);
            // adding the list of books 
            foreach (BookDTO book in StandardLibrary)
            {
                await AddBook(book);
            }
            await _bookRepository.SaveChanges();
            return true;

        }

    }
}

