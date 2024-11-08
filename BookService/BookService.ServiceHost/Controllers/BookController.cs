using BookService.Application.Handlers.CreateBook;
using BookService.Application.Handlers.GetBook;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.Book;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.ServiceHost.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AddBook([FromBody] BookRequest request, CancellationToken cancellation)
    {
        var command = new CreateBookCommand
        {
            Title = request.Title,
            AuthorsId = request.AuthorsIds
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created);


        var error = new GenericError
        {
            Description = result.Error.Description
        };

        return result.Error.Reason switch
        {
            ErrorReason.BadRequest => StatusCode(StatusCodes.Status400BadRequest, error),
            _ => StatusCode(StatusCodes.Status500InternalServerError, error)
        };
    }

    [HttpGet]
    public async Task<ActionResult<PaginationWrapper<BookResponse>>> GetManyBooks(CancellationToken cancellation,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1,
        [FromQuery] bool includeBookAuthors = false,
        [FromQuery] string? title = null)
    {
        var command = new GetManyBooksCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            InludeAuthorDetails = includeBookAuthors,
            Title = title
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var books = result.Value
                .Select(e => e.ToDto())
                .GetPaginationResult(pageNumber, pageSize);

            return StatusCode(StatusCodes.Status200OK, books);
        };

        var error = new GenericError
        {
            Description = result.Error.Description
        };

        return result.Error.Reason switch
        {
            ErrorReason.BadRequest => StatusCode(StatusCodes.Status400BadRequest, error),
            _ => StatusCode(StatusCodes.Status500InternalServerError, error)
        };
    }
}
