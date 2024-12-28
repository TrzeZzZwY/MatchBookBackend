using BookService.Application.Handlers.CreateAuthor;
using BookService.Application.Handlers.DeleteAuthor;
using BookService.Application.Handlers.EditAuhor;
using BookService.Application.Handlers.GetAuthor;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.Author;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController: ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AddAuthor([FromBody] AuthorRequest request, CancellationToken cancellation)
    {
        var command = new CreateAuthorCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            YearOfBirth = request.YearOfBirth,
            Country = request.Country
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
    public async Task<ActionResult<PaginationWrapper<AuthorResponse>>> GetAllAuthors(
        CancellationToken cancellation,
        [FromQuery] bool showRemoved = false,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1)
    {
        var command = new GetManyAuthorsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            },
            ShowRemoved = showRemoved
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var authors = result.Value
                .Select(e => e.ToDto())
                .GetPaginationResult(pageNumber, pageSize);

            return StatusCode(StatusCodes.Status200OK, authors);
        }

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

    [HttpGet("{authorId:int}")]
    public async Task<ActionResult<AuthorResponse>> GetAuthorById(
        [FromRoute] int authorId,
        CancellationToken cancellation)
    {
        var command = new GetAuthorCommand
        {
            AuthorId = authorId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return result.Value is null ?
                NotFound() :
                Ok(result.Value.ToDto());
        }

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

    [HttpDelete("{authorId:int}")]
    public async Task<ActionResult> SoftDeleteAuthor([FromRoute] int authorId, CancellationToken cancellation)
    {
        //TODO: Only for admin
        var command = new DeleteAuthorCommand
        {
            AuthorId = authorId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return NoContent();
        }

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

    [HttpPut("{authorId:int}")]
    public async Task<ActionResult> EditAuthor([FromRoute] int authorId, [FromBody] AuthorRequest request, CancellationToken cancellation)
    {
        //Only for admin
        var command = new EditAuthorCommand
        {
            AuthorId = authorId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Country = request.Country,
            YearOfBirth = request.YearOfBirth
        };
        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return Ok();
        }

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
