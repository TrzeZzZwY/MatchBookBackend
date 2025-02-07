using AccountService.Domain.Common;
using AccountService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.UserStatus;
public class ChanageUserStatusHandler : IRequestHandler<ChangeUserStatusCommand, Result<ChangeUserStatusResult, Error>>
{
    private readonly DatabaseContext _context;

    public ChanageUserStatusHandler(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }
    public async Task<Result<ChangeUserStatusResult, Error>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.UserAccounts.FindAsync([request.UserId], cancellationToken);

        if (user is null) return new Error("User not found", ErrorReason.NotFound);

        await _context.Entry(user).Reference(e => e.Account).LoadAsync(cancellationToken);

        var updateStatusResult = user.Account.ChangeAccountStatus(request.Status);

        await _context.SaveChangesAsync(cancellationToken);

        return updateStatusResult.IsSuccess ?
            new ChangeUserStatusResult() :
            updateStatusResult.Error;
    }
}
