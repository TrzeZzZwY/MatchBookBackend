using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetBookPointHandler : IRequestHandler<GetBookPointCommand, Result<GetBookPointResult?, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetBookPointHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetBookPointResult?, Error>> Handle(GetBookPointCommand request, CancellationToken cancellationToken)
    {
        var bookPoint = await _databaseContext.BookPoints.FindAsync([request.BookPointId], cancellationToken);
        return bookPoint?.ToHandlerResult();
    }
}
