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
    public async Task<ActionResult<CreateEntityResponse>> AddAuthor([FromBody] AuthorRequest request, CancellationToken cancellation)
    {
        var command = new CreateAuthorCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            YearOfBirth = request.YearOfBirth,
            Country = request.Country
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess) return StatusCode(StatusCodes.Status201Created, new CreateEntityResponse { Id = result.Value.AuthorId});

        return result.Error.ToErrorResult();
    }

    [HttpGet]
    public async Task<ActionResult<PaginationWrapper<AuthorResponse>>> GetAllAuthors(
        CancellationToken cancellation,
        [FromQuery] string? authorName = null,
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
            ShowRemoved = showRemoved,
            AuthorName = authorName
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var authors = result.Value.GetPaginationResult(DtoExtensions.ToDto);

            return StatusCode(StatusCodes.Status200OK, authors);
        }

        return result.Error.ToErrorResult();
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

        return result.Error.ToErrorResult();
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

        return result.Error.ToErrorResult();
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

        return result.Error.ToErrorResult();
    }
}
