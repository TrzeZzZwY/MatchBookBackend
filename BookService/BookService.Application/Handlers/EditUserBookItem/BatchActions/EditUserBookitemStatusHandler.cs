using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.EditUserBookItem.BatchActions;
public class EditUserBookitemStatusHandler : IRequestHandler<EditAllUserBookItemsRegionCommand, Result<EditAllUserBookItemsRegionResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditUserBookitemStatusHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditAllUserBookItemsRegionResult, Error>> Handle(EditAllUserBookItemsRegionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userBooks = _databaseContext.UserBookItems.AsQueryable();

            userBooks = userBooks.Where(e => e.UserId == request.UserId);

            await userBooks.ExecuteUpdateAsync(e => e.SetProperty(e => e.Region, e => request.Region));

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditAllUserBookItemsRegionResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
