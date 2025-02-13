using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.CreateCase;
public class CreateCaseCommand : IRequest<Result<CreateCaseResult, Error>>
{
    public required int UserId { get; set; }

    public required CaseItemType Type { get; set; }

    public required Dictionary<string, string> CaseFields { get; set; }

    public required int ItemId { get; set; }

    public required string Notes { get; set; }

    public required ReportType ReportType { get; set; }
}
