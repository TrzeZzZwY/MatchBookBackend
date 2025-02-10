using BookService.Application.Handlers.GetMatches;
using BookService.Domain.Common;
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
    public async Task<ActionResult<MatchesResponse>> GetMatchesForUser(CancellationToken cancellation,
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
                Matches = result.Value.Matches.Select(e => new MatchResponse
                {
                    UserId = e.UserId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MatchBookPair = e.Items.Select(r => new MatchBookPairResponse
                    {
                        OfferedBook = new BookItemResponse
                        {
                            Title = r.OfferedBook.Title,
                            ImageId = r.OfferedBook.ImageId,
                            UserBookItemId = r.OfferedBook.UserBookItemId
                        },
                        RequestedBook = new BookItemResponse
                        {
                            Title = r.RequestedBook.Title,
                            ImageId = r.RequestedBook.ImageId,
                            UserBookItemId = r.OfferedBook.UserBookItemId
                        },
                    }).ToList()
                }).ToList()
            };

            return StatusCode(StatusCodes.Status200OK, userLikes);
        };

        return result.Error.ToErrorResult();
    }
}
