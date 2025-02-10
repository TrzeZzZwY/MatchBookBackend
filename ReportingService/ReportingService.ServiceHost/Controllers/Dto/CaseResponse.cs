using ReportingService.Domain.Common;

namespace ReportingService.ServiceHost.Controllers.Dto;

public class CaseResponse
{
    public required int CaseId { get; init; }

    public required int UserId { get; init; }

    public required int ItemId { get; init; }

    public required CaseStatus CaseStatus { get; init; }

    public required CaseItemType CaseType { get; init; }

    public required ReportType ReportType { get; init; }

    public required string ReportNote { get; init; }

    public int? ReviewerId { get; init; }

    public required Dictionary<string, string> keyValues { get; init; }
}
