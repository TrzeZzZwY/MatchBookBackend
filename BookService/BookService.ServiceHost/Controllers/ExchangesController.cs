using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto.Book;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookService.Application.Handlers.Exchange.GetExchanges;
using BookService.ServiceHost.Controllers.Dto.Exchanges;
using BookService.Application.Handlers.Exchange.CreateExchange;
using BookService.Application.Handlers.Exchange.AcceptExchange;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExchangesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExchangesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("Admin")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<PaginationWrapper<BookResponse>>> GetExchanges(CancellationToken cancellation,
    [FromQuery] int pageSize = 50,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int? initiatorUserId = null,
    [FromQuery] int? receiverUserId = null,
    [FromQuery] ExchangeStatus? status = null)
    {
        var command = new GetExchangesCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            InitiatorUserId = initiatorUserId,
            ReceiverUserId = receiverUserId,
            Status = status
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var books = result.Value.GetPaginationResult(DtoExtensions.ToDto);
            return StatusCode(StatusCodes.Status200OK, books);
        };

        return result.Error.ToErrorResult();
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<PaginationWrapper<BookResponse>>> GetExchanges(CancellationToken cancellation,
    [FromQuery] int pageSize = 50,
    [FromQuery] int pageNumber = 1)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);

        var command = new GetExchangesCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            ReceiverUserId = (int)userId,
            Status = ExchangeStatus.Pending
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var books = result.Value.GetPaginationResult(DtoExtensions.ToDto);
            return StatusCode(StatusCodes.Status200OK, books);
        };

        return result.Error.ToErrorResult();
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<PaginationWrapper<BookResponse>>> AddExchange([FromBody] AddExchangeRequest request, CancellationToken cancellation)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);

        var command = new CreateExchangeCommand
        {
            InitiatorUserId = (int)userId,
            InitiatorBookItemId = request.InitiatorBookItemId,
            ReceiverBookItemId = request.ReceiverBookItemId,
            ReceiverUserId = request.ReceiverUserId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status201Created);
        };

        return result.Error.ToErrorResult();
    }

    [HttpPut]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<PaginationWrapper<BookResponse>>> RespondtoExchanges([FromBody] RespondtoExchangesRequest request, CancellationToken cancellation)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);

        var command = new RespondToExchangeCommand
        {
            UserId = (int)userId,
            Accepted = request.Accepted,
            ExchangeId = request.ExchangeId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status200OK);
        };

        return result.Error.ToErrorResult();
    }
}
