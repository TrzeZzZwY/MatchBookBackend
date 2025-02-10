using CSharpFunctionalExtensions;
using ReportingService.Application.Clients;
using ReportingService.Domain.Common;
using ReportingService.Domain.Models;

namespace ReportingService.Application.Strategies.RejectCase;
public class RejectBookCaseItemStrategy : IStrategy<CaseEntity>
{
    private readonly BookServiceClient _bookServiceClient;

    public RejectBookCaseItemStrategy(BookServiceClient bookServiceClient)
    {
        _bookServiceClient = bookServiceClient;
    }

    public bool CanHandle(CaseEntity item) => item.CaseItemType == CaseItemType.BOOK;

    public async Task<Result<bool, Error>> HandleAsync(CaseEntity item, CancellationToken cancellation)
    {
        var deleteResult = await _bookServiceClient.DeleteBook(item.ItemId);
        return deleteResult.IsSuccess ? true : deleteResult.Error;
    }
}
