using CSharpFunctionalExtensions;
using ReportingService.Application.Clients;
using ReportingService.Domain.Common;
using ReportingService.Domain.Models;

namespace ReportingService.Application.Strategies.RejectCase;
public class RejectUserCaseItemStrategy : IStrategy<CaseEntity>
{
    private readonly AccountServiceClient _accountServiceClient;

    public RejectUserCaseItemStrategy(AccountServiceClient accountServiceClient)
    {
        _accountServiceClient = accountServiceClient;
    }

    public bool CanHandle(CaseEntity item) => item.CaseItemType == CaseItemType.USER;

    public async Task<Result<bool, Error>> HandleAsync(CaseEntity item, CancellationToken cancellation)
    {
        var deleteResult = await _accountServiceClient.BanUser(item.UserId);
        return deleteResult.IsSuccess ? true : deleteResult.Error;
    }
}
