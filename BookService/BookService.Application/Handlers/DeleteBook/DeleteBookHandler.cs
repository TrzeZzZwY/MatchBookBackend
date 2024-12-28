using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteBook;
public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, Result<DeleteBookResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public DeleteBookHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<DeleteBookResult, Error>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var book = _databaseContext.Books.Find(request.BookId);
            if (book is null) return new DeleteBookResult();

            book.IsDeleted = true;
            await _databaseContext.Entry(book).Collection(e => e.BookItems).LoadAsync();
            foreach (UserBookItem item in book.BookItems)
            {
                var updateResult = item.UpdateStatus(UserBookItemStatus.Removed);
                item.UpdateDate = DateTime.UtcNow;
                if (updateResult.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return updateResult.Error;
                }
            }

            await _databaseContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new DeleteBookResult();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}