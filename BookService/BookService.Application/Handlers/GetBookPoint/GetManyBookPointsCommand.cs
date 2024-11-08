using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetManyBookPointsCommand : IRequest<Result<List<GetBookPointResult>, Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public Region? Region { get; set; }
}
