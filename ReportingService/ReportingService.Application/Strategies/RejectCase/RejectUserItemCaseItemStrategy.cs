using CSharpFunctionalExtensions;
using ReportingService.Application.Clients;
using ReportingService.Domain.Common;
using ReportingService.Domain.Models;

namespace ReportingService.Application.Strategies.RejectCase;
public class RejectUserItemCaseItemStrategy : IStrategy<CaseEntity>
{
    private readonly BookServiceClient _bookServiceClient;

    public RejectUserItemCaseItemStrategy(BookServiceClient bookServiceClient)
    {
        _bookServiceClient = bookServiceClient;
    }

    public bool CanHandle(CaseEntity item) => item.CaseItemType == CaseItemType.USERITEM;

    public async Task<Result<bool, Error>> HandleAsync(CaseEntity item, CancellationToken cancellation)
    {
        var deleteResult = await _bookServiceClient.DeleteUserItem(item.ItemId);
        return deleteResult.IsSuccess ? true : deleteResult.Error;
    }
}
