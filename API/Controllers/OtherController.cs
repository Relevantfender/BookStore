﻿using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Exceptions
{
    /// <summary>
    /// Other operation management
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class OtherController :ControllerBase
    {
        private readonly IBookService _bookService;

        public OtherController(IBookService bookService)
        {
            _bookService = bookService;
        }
        /// <summary>
        /// Resets the table and enters 30 books from Json
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetDatabase()
        {
            await _bookService.ResetBooks();
            return Ok(new BookResponse
            {
                StatusCode = StatusCodes.Status200OK,
                ResponseMessage = "Books have been successfully reset"
            });
        }


    }
}
