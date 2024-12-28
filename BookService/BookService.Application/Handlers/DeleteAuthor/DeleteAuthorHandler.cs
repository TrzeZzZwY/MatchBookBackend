using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteAuthor;
public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, Result<DeleteAuthorResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public DeleteAuthorHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    public async Task<Result<DeleteAuthorResult, Error>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var author = _databaseContext.Authors.Find(request.AuthorId);
            if (author is null) return new DeleteAuthorResult();

            author.IsDeleted = true;
            await _databaseContext.Entry(author).Collection(e => e.AuthorBooks).LoadAsync();
            foreach (Book book in author.AuthorBooks)
            {
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
            }

            await _databaseContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new DeleteAuthorResult();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
