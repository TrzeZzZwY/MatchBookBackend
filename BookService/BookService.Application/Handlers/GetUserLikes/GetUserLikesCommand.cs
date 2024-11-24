using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserLikes;
public class GetUserLikesCommand : IRequest<Result<GetUserLikesResult, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public required int UserId { get; set; }
}
