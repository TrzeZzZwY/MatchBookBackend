using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditUserBookItem;
public class EditUserBookItemHandler : IRequestHandler<EditUserBookItemCommand, Result<EditUserBookItemResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditUserBookItemHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditUserBookItemResult, Error>> Handle(EditUserBookItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var item = await _databaseContext.UserBookItems.FindAsync([request.UserBookItemId], cancellationToken);
            if (item is null) return new Error($"UserBookItem not found for userBookItemId: {request.UserBookItemId}", ErrorReason.BadRequest);

            var updateStatusResult = item.UpdateStatus(request.Status);
            if (updateStatusResult.IsFailure) return updateStatusResult.Error;

            var bookReference = await _databaseContext.Books.FindAsync([request.BookReferenceId], cancellationToken);
            if (bookReference is null) return new Error($"Book not found for bookReferenceId: {request.BookReferenceId}", ErrorReason.BadRequest);

            item.Description = request.Description;
            item.BookReferenceId = request.BookReferenceId;
            item.BookPointId = request.BookPointId;

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditUserBookItemResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
