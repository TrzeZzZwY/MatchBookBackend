using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditBookPoint;
public class EditBookPointHandler : IRequestHandler<EditBookPointCommand, Result<EditBookPointResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditBookPointHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditBookPointResult, Error>> Handle(EditBookPointCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var bookPoint = await _databaseContext.BookPoints.FindAsync([request.BookPointId], cancellationToken);
            if (bookPoint is null) return new Error($"BookPoint not found for id {request.BookPointId}", ErrorReason.BadRequest);

            bookPoint.Lat = request.Lat;
            bookPoint.Long = request.Long;
            bookPoint.Capacity = request.Capacity;
            bookPoint.Region = request.Region;
            bookPoint.UpdateDate = DateTime.UtcNow;

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditBookPointResult();
        }
        catch(Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
