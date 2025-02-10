using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.GetCasesList;
public class GetCasesListItem
{
    public required int CaseId { get; init; }

    public required int UserId { get; init; }

    public required CaseStatus CaseStatus { get; init; }

    public required CaseItemType CaseType { get; init; }

    public required ReportType ReportType { get; init; }

    public int? ReviewerId { get; init; }
}
