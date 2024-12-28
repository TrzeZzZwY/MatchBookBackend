using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteBookPoint;
public class DeleteBookPointHandler : IRequestHandler<DeleteBookPointCommand, Result<DeleteBookPointResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public DeleteBookPointHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<DeleteBookPointResult, Error>> Handle(DeleteBookPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bookPoint = await _databaseContext.BookPoints.FindAsync([request.BookPointId], cancellationToken);
            if (bookPoint is null) return new DeleteBookPointResult();

            bookPoint.IsDeleted = true;
            bookPoint.UpdateDate = DateTime.UtcNow;

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new DeleteBookPointResult();
        }
        catch(Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
