﻿using BookService.Application.Handlers.GetMatches;
using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookService.ServiceHost.Controllers.Dto.Match;
using BookService.ServiceHost.Extensions;
using Microsoft.AspNetCore.Authorization;

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
    [Authorize(Roles = "User")]
    public async Task<ActionResult> GetMatchesForUser(CancellationToken cancellation,
                [FromQuery] int pageSize = 50,
                [FromQuery] int pageNumber = 1)
    {
        var userId = User.GetId();
        if (userId is null) return StatusCode(StatusCodes.Status400BadRequest);
        var command = new GetMatchesCommand
        {
            PaginationOptions = new PaginationOptions
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            },
            UserId = (int)userId
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

        return result.Error.ToErrorResult();
    }
}
