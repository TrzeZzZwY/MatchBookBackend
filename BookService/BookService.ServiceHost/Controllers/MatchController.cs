﻿using BookService.Application.Handlers.GetMatches;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto.UserLikes;
using BookService.ServiceHost.Controllers.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookService.ServiceHost.Controllers.Dto.Match;

namespace BookService.ServiceHost.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetMatchesForUser(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int userId = 1)
    {
        var command = new GetMatchesCommand
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
            var userLikes = new MatchesResponse
            {
                MatchedUsers = result.Value.MatchedUsersIds
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