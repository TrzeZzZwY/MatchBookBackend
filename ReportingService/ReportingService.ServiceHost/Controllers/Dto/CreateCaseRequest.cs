using ReportingService.Domain.Common;

namespace ReportingService.ServiceHost.Controllers.Dto;

public class CreateCaseRequest
{
    public int UserId { get; init; }

    public CaseItemType Type { get; init; }

    public Dictionary<string, string> CaseFields { get; init; }

    public int ItemId { get; init; }
}
