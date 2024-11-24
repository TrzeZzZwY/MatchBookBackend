using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetMatches;
public class GetMatchesCommand : IRequest<Result<GetMatchesResult, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public required int UserId { get; set; }
}