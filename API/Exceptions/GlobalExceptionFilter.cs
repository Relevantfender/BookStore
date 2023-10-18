using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class GlobalExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        HttpStatusCode statusCode;
        string errorMessage;

        switch (context.Exception)
        {
            case BadRequestException _:
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "Bad request.";
                break;

            case ConflictException _:
                statusCode = HttpStatusCode.Conflict;
                errorMessage = "Conflict.";
                break;

            case InvalidInputException _:
                statusCode = HttpStatusCode.UnprocessableEntity;
                errorMessage = "Invalid input.";
                break;

            case NotFoundException _:
                statusCode = HttpStatusCode.NotFound;
                errorMessage = "Book not found.";
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                errorMessage = "An error occurred.";
                break;
        }

        // Create a JSON response with the appropriate status code and error message
        context.Result = new JsonResult(new { error = errorMessage }) { StatusCode = (int)statusCode };
    }
}

public class BadRequestException : Exception { }
public class ConflictException : Exception { }
public class InvalidInputException : Exception { }
public class NotFoundException : Exception { }
