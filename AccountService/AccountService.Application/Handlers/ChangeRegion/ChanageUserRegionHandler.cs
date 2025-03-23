using AccountService.Application.Clients;
using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.ChangeRegion;
public class ChanageUserRegionHandler : IRequestHandler<ChangeUserRegionCommand, Result<ChangeUserRegionResult, Error>>
{
    private readonly DatabaseContext _context;
    private readonly IBookServiceClient _bookServiceClient;

    public ChanageUserRegionHandler(DatabaseContext databaseContext, IBookServiceClient bookServiceClient)
    {
        _context = databaseContext;
        _bookServiceClient = bookServiceClient;
    }
    public async Task<Result<ChangeUserRegionResult, Error>> Handle(ChangeUserRegionCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserAccounts.FindAsync([request.UserId], cancellationToken);

        if (user is null) return new Error("User not found", ErrorReason.NotFound);

        user.Region = request.region;

        await _context.SaveChangesAsync(cancellationToken);

        var updateItemsResult = await _bookServiceClient.UpdateItemsRegion();
        if (updateItemsResult.IsFailure) return updateItemsResult.Error;

        return new ChangeUserRegionResult();
    }
}
