using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.ToggleLike;
public class ToggleLikeCommand : IRequest<Result<ToggleLikeResult, Error>>
{
    public required int UserId { get; set; }

    public required int UserBookItemId { get; set; }
}

