using API.Data;
using API.DTO;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    
    [ApiController]
    [Route("[controller]")]
        public class BooksController : ControllerBase
        {
            private readonly IBookService _bookService;
            private readonly IMapper _mapper;
            private readonly IAuthorService _authorService;

        public BooksController(IBookService bookService, IMapper mapper, IAuthorService authorService)
        {
            _bookService = bookService;
            _mapper = mapper;
            _authorService = authorService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks(string title, string author,)
        {
            var books = await _bookService.GetBooks();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> PostBook([FromBody] BookDTO bookDTO)
        {
            await _bookService.AddBook(bookDTO);

            return Ok(bookDTO);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDTO>> GetBookByID(int id) {
            return await _bookService.GetBookByID(id);
        
        }

      
     
        

       
    }
}
