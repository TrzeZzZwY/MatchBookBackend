using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;
using System.Text.Json;

namespace ReportingService.Application.Handlers.GetCase;
public class GetCaseHandler : IRequestHandler<GetCaseCommand, Result<GetCaseResult?, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetCaseHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetCaseResult?, Error>> Handle(GetCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await _databaseContext.Cases.FindAsync([request.CaseId], cancellationToken);
        if (caseEntity is null) return (GetCaseResult)null;

        var keyValues = JsonSerializer.Deserialize<Dictionary<string, string>>(caseEntity.SerializedCaseFields);
        if (keyValues is null) return new Error("Cannot deserialise case fields", ErrorReason.InternalError);

        return new GetCaseResult
        {
            CaseId = caseEntity.Id,
            UserId = caseEntity.UserId,
            CaseStatus = caseEntity.CaseStatus,
            CaseType = caseEntity.CaseItemType,
            keyValues = keyValues,
            ItemId = caseEntity.ItemId,
            ReportNote = caseEntity.ReportNote,
            ReportType = caseEntity.ReportType,
            ReviewerId = caseEntity.ReviewerId
        };
    }
}
