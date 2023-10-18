using API.Data;
using API.DTO;
using API.DTO.Filters;
using API.Entities;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Exceptions
{

    /// <summary>
    /// Books Management
    /// </summary>
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
        /// <summary>
        /// Get a list of books
        /// </summary>
        /// <param name="pageRequest"></param>
        /// <returns>Return all specific set using filters</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="400">Bad request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBooks([FromQuery] PageRequest pageRequest)
        {
            List<BookDTO> books = await _bookService.GetBooks(pageRequest);
            return Ok(books);
        }
        /// <summary>
        /// Add a new book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>Return all specific set using filters</returns>
        /// <response code="201">Successfull</response>
        /// <response code="409">Conflict</response>
        /// <response code="422">Invalid input</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> PostBook([FromBody] BookDTO bookDTO)
        {
            // Check for missing or invalid parameters and returns 422
            if (bookDTO.Title.Equals(null) || bookDTO.Isbn.Equals(0) || bookDTO.NumberOfPages.Equals(0))
            {
                var validationBook = new ValidationBook
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };

                if (bookDTO.Title == null || bookDTO.Title.Equals("string"))
                {
                    validationBook.ResponseMessage.Add("Title is missing.");
                }

                if (bookDTO.Isbn == 0)
                {
                    validationBook.ResponseMessage.Add("ISBN is missing or invalid.");
                }

                if (bookDTO.NumberOfPages == 0)
                {
                    validationBook.ResponseMessage.Add("Number of pages is missing or invalid.");
                }

                if (validationBook.ResponseMessage.Any())
                {
                    return UnprocessableEntity(validationBook);
                }
            }

            Book newBook = await _bookService.AddBook(bookDTO);
            //handles conflict response
            if (newBook == null)
            {
                return Conflict(new BookResponse
                {
                    StatusCode = (int)HttpStatusCode.Conflict,
                    ResponseMessage = "That book already exists"
                });
            }
            // handles book creation
            else return Created(string.Empty, new BookResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                ResponseMessage = "Book successfully created"
            });
        }

        /// <summary>
        /// Return a specific book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Successful operation</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookDTO>> GetBookByID(int id)
        {
            return await _bookService.GetBookByID(id);
        }
        /// <summary>
        /// Updated a specific book
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        /// <response code="200">Updated</response>
        /// <response code="404">Not found</response>
        /// <response code="409">Conflict</response>
        /// <response code="422">Invalid input</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<BookDTO>> UpdateBookByID(int id, [FromBody] BookDTO bookDTO)
        {
            if (bookDTO == null)
                return UnprocessableEntity(new BookResponse
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity,
                    ResponseMessage = "No data was inserted"
                });
            //returned value if the book is updated
            var bookUpdated = await _bookService.UpdateBookByID(id, bookDTO);
            //if true it's updated, if false it's not found
            if (!bookUpdated)
                return NotFound(new BookResponse
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ResponseMessage = "Book Not found"
                });
            else return Ok(new BookResponse
            {
                StatusCode = StatusCodes.Status200OK,
                ResponseMessage = "Book Updated successfully"
            });
        }

        /// <summary>
        /// Delete a specific book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Deleted</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteBookByID(int id)
        {
            var book = await _bookService.GetBookByID(id);
            if (book == null)
            {
                throw new NotFoundException();
            }
            await _bookService.DeleteBookByID(id);
            return Ok(new BookResponse
            {
                StatusCode = StatusCodes.Status200OK,
                ResponseMessage = "Book has been successfully deleted"
            });

        }


    }
}