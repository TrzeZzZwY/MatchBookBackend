using BookService.Application.Handlers.CreateBookPoint;
using BookService.Application.Handlers.DeleteBookPoint;
using BookService.Application.Handlers.EditBookPoint;
using BookService.Application.Handlers.GetBookPoint;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.Book;
using BookService.ServiceHost.Controllers.Dto.BookPoint;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookPointController : ControllerBase
{
    private readonly IMediator _mediator;

    public BookPointController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AddBookPoint([FromBody] BookPointRequest request, CancellationToken cancellation)
    {
        var command = new CreateBookPointCommand
        {
            Region = request.Region,
            Lat = request.Lat,
            Long = request.Long,
            Capacity = request.Capacity
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
    public async Task<ActionResult<PaginationWrapper<BookPointResponse>>> GetManyBookPoints(CancellationToken cancellation,
        [FromQuery] int pageSize = 50,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int? region = null)
    {
        var command = new GetManyBookPointsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            Region = (Region?)region
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var bookPoints = result.Value
                .Select(e => e.ToDto())
                .GetPaginationResult(pageNumber, pageSize);

            return StatusCode(StatusCodes.Status200OK, bookPoints);
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

    [HttpGet("{bookPointId:int}")]
    public async Task<ActionResult<BookPointResponse>> GetById([FromRoute] int bookPointId, CancellationToken cancellation)
    {
        var command = new GetBookPointCommand
        { 
            BookPointId = bookPointId 
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

    [HttpDelete("{bookPointId:int}")]
    public async Task<ActionResult> SoftDelete([FromRoute] int bookPointId, CancellationToken cancellation)
    {
        //Only for admin
        var command = new DeleteBookPointCommand
        {
            BookPointId = bookPointId
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

    [HttpPut("{bookPointId:int}")]
    public async Task<ActionResult> Edit([FromRoute] int bookPointId, [FromBody] BookPointRequest request, CancellationToken cancellation)
    {
        var command = new EditBookPointCommand
        {
            BookPointId = bookPointId,
            Capacity = request.Capacity,
            Lat = request.Lat,
            Long = request.Long,
            Region = request.Region
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
