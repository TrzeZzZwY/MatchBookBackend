using BookService.Application.Handlers.CreateAuthor;
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
    public async Task<ActionResult<PaginationWrapper<AuthorResponse>>> GetAllAuthors(CancellationToken cancellation, [FromQuery] int pageSize = 50, [FromQuery] int pageNumber = 1)
    {
        var command = new GetManyAuthorsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageSize = pageSize,
                PageNumber = pageNumber
            }
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var a = new GetAuthorResult
            {
                Country = "asd",
                FistName = "as",
                Id = 1,
                LastName = "asd",
                YearOfBirth = 1342
            };

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
}
