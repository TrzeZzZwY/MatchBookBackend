using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditUserBookItem.AdminActions;
public class EditUserBookitemStatusHandler : IRequestHandler<EditUserBookItemStatusCommand, Result<EditUserBookItemStatusResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditUserBookitemStatusHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditUserBookItemStatusResult, Error>> Handle(EditUserBookItemStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _databaseContext.UserBookItems.FindAsync([request.UserBookItemId], cancellationToken);
            if (item is null) return new Error($"UserBookItem not found for userBookItemId: {request.UserBookItemId}", ErrorReason.BadRequest);

            if (item.Status != request.Status)
            {
                var updateStatusResult = item.UpdateStatus(request.Status);
                if (updateStatusResult.IsFailure) return updateStatusResult.Error;
            }
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditUserBookItemStatusResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
