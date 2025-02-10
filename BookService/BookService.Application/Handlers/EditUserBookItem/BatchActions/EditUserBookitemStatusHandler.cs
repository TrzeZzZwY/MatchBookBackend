using BookService.Application.Clients;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.EditUserBookItem.BatchActions;
public class EditUserBookitemStatusHandler : IRequestHandler<EditAllUserBookItemsRegionCommand, Result<EditAllUserBookItemsRegionResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly AccountServiceClient _accountServiceClient;

    public EditUserBookitemStatusHandler(DatabaseContext databaseContext, AccountServiceClient accountServiceClient)
    {
        _databaseContext = databaseContext;
        _accountServiceClient = accountServiceClient;
    }

    public async Task<Result<EditAllUserBookItemsRegionResult, Error>> Handle(EditAllUserBookItemsRegionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _accountServiceClient.FetchUser(request.UserId);
            if (user.IsFailure) return user.Error;

            var userBooks = _databaseContext.UserBookItems.AsQueryable();

            userBooks = userBooks.Where(e => e.UserId == request.UserId);

            await userBooks.ExecuteUpdateAsync(e => e.SetProperty(e => e.Region, e => user.Value.Region));

            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditAllUserBookItemsRegionResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
