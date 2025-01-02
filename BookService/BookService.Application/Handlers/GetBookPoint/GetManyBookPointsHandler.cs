using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetManyBookPointsHandler : IRequestHandler<GetManyBookPointsCommand, Result<PaginatedResult<GetBookPointResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyBookPointsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetBookPointResult>, Error>> Handle(GetManyBookPointsCommand request, CancellationToken cancellationToken)
    {
        var bookPoints = _databaseContext.BookPoints.AsQueryable();

        if (request.Region is not null)
            bookPoints = bookPoints.Where(e => e.Region == request.Region);

        var total = bookPoints.Count();
        bookPoints = bookPoints
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(
            bookPoints.ToList().Select(e => e.ToHandlerResult()).ToList(),
            total
            );
    }
}