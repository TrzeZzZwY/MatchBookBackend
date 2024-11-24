﻿using BookService.Application.Handlers.CreateUserBookItem;
using BookService.Application.Handlers.GetUserBookItems;
using BookService.Application.Handlers.GetUserLikes;
using BookService.Application.Handlers.ToggleLike;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using BookService.ServiceHost.Controllers.Dto.UserItemBook;
using BookService.ServiceHost.Controllers.Dto.UserLikes;
using BookService.ServiceHost.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserBookItemController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserBookItemController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> AddUserItemBook([FromBody] UserBookItemRequest request, CancellationToken cancellation)
    {
        var command = new CreateUserBookItemCommand
        {
            UserId = request.UserId,
            BookReferenceId = request.BookReferenceId,
            BookPointId = request.BookPointId,
            Description = request.Description,
            Status = request.Status
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
    public async Task<ActionResult<PaginationWrapper<UserBookItemResponse>>> GetManyUserBookItem(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1)
    {
        var command = new GetManyUserBookItemsCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            }
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

    [HttpPost("toggle-like")]
    public async Task<ActionResult> ToggleLikeUserItem([FromBody] UserLikeBookRequest request, CancellationToken cancellation)
    {
        var command = new ToggleLikeCommand { UserBookItemId = request.UserBookItemId, UserId = request.UserId };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
            return StatusCode(StatusCodes.Status200OK);

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

    [HttpGet("get-like")]
    public async Task<ActionResult<UserLikesResponse>> GetUserLikes(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int userId = 1)
    {
        var command = new GetUserLikesCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            UserId = userId
        };

        var result = await _mediator.Send(command, cancellation);

        if (result.IsSuccess)
        {
            var userLikes = new UserLikesResponse
            {
                UserId = result.Value.UserId,
                UserLikes = result.Value.Items.Select(e => e.ToDto()).GetPaginationResult(pageNumber, pageSize)
            };

            return StatusCode(StatusCodes.Status200OK, userLikes);
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
