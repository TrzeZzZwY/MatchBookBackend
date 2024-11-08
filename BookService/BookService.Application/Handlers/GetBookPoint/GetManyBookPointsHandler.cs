using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetManyBookPointsHandler : IRequestHandler<GetManyBookPointsCommand, Result<List<GetBookPointResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyBookPointsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetBookPointResult>, Error>> Handle(GetManyBookPointsCommand request, CancellationToken cancellationToken)
    {
        var bookPoints = _databaseContext.BookPoints.AsQueryable();

        if (request.Region is not null)
            bookPoints = bookPoints.Where(e => e.Region == request.Region);

        bookPoints = bookPoints
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return bookPoints.ToList().Select(e => new GetBookPointResult
        {
            Id = e.Id,
            Lat = e.Lat,
            Long = e.Lat,
            Capacity = e.Capacity,
            Region = e.Region
        }).ToList();
    }
}
