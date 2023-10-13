using API.Data;
using API.DTO;
using API.DTO.Filters;
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
        public IActionResult GetBooks([FromQuery] PageRequest pageRequest)
        {
            IEnumerable<BookDTO> books = _bookService.GetBooks(pageRequest);
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
        [HttpPatch("{id}")]
        public async Task<ActionResult<BookDTO>> UpdateBookByID(int id, [FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null)
                return UnprocessableEntity("Invalid input");
            //returned value if the book is updated
            var bookUpdated = await _bookService.UpdateBookByID(id, bookDTO);

            if (!bookUpdated)
                return NotFound();
            var updatedBook = await _bookService.GetBookByID(id);
            //object of the updated book mapped to dto
            return Ok(_mapper.Map<BookDTO>(updatedBook));
        }

        /// <summary>
        /// deletes a specific book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBookByID(int id)
        {
            try
            {
                var book = await _bookService.GetBookByID(id);
                if (book == null)
                {
                    return NotFound();
                }

                return await _bookService.DeleteBookByID(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        

    }
        

      
     
        

       
    
}
