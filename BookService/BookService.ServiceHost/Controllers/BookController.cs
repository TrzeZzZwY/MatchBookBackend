using BookService.Application.Handlers.CreateBook;
using BookService.Application.Handlers.DeleteBook;
using BookService.Application.Handlers.EditBook;
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
        [FromQuery] bool showRemoved = false,
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
            Title = title,
            ShowRemoved = showRemoved
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

    [HttpGet("{bookId:int}")]
    public async Task<ActionResult<BookResponse>> GetById(CancellationToken cancellation, [FromRoute] int bookId, [FromQuery] bool includeBookAuthors = false)
    {
        var command = new GetBookCommand
        {
            BookId = bookId,
            InludeAuthorDetails = includeBookAuthors
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return result.Value is null ?
                NotFound() :
                Ok(result.Value.ToDto());
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

    [HttpDelete("{bookId:int}")]
    public async Task<ActionResult> SoftDelete([FromRoute] int bookId, CancellationToken cancellation)
    {
        //Only for admin
        var command = new DeleteBookCommand
        {
            BookId = bookId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return NoContent();
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

    [HttpPut("{bookId:int}")]
    public async Task<ActionResult> Edit([FromRoute] int bookId, [FromBody] BookRequest request, CancellationToken cancellation)
    {
        //Only for admin
        var command = new EditBookCommand
        {
            BookId = bookId,
            Title = request.Title,
            AuthorsId = request.AuthorsIds
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return Ok();
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
