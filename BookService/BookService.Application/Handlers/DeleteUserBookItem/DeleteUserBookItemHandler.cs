using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteUserBookItem;
public class DeleteUserBookItemHandler : IRequestHandler<DeleteUserBookItemCommand, Result<DeleteUserBookItemResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public DeleteUserBookItemHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<DeleteUserBookItemResult, Error>> Handle(DeleteUserBookItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _databaseContext.UserBookItems.FindAsync([request.UserBookItemId], cancellationToken);
            if (item is null) return new DeleteUserBookItemResult();

            var updateResult = item.UpdateStatus(UserBookItemStatus.Removed);
            item.UpdateDate = DateTime.UtcNow;
            if (updateResult.IsFailure) return updateResult.Error;

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new DeleteUserBookItemResult();

        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}