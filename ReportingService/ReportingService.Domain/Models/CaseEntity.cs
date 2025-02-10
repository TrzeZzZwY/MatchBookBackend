using CSharpFunctionalExtensions;
using ReportingService.Domain.Common;

namespace ReportingService.Domain.Models;
public class CaseEntity
{
    public CaseEntity(int userId, CaseItemType caseItemType, string serializedCaseFields, int itemId, string reportNote, ReportType reportType)
    {
        UserId = userId;
        CaseItemType = caseItemType;
        SerializedCaseFields = serializedCaseFields;
        ItemId = itemId;
        ReportNote = reportNote;
        ReportType = reportType;
    }

    public int Id { get; private set; }

    public int UserId { get; private set; }

    public int? ReviewerId { get; private set; }

    public int ItemId { get; private set; }

    public CaseStatus CaseStatus { get; private set; }

    public CaseItemType CaseItemType { get; private set; }

    public string SerializedCaseFields { get; private set; }

    public ReportType ReportType { get; private set; }

    public string ReportNote { get; private set; }

    //data

    public Result<CaseEntity,Error> AssignReviewer(int reviewerId)
    {
        if (ReviewerId is not null) return new Error("Reviewer already assinged", ErrorReason.InvalidOperation);

        ReviewerId = reviewerId;
        CaseStatus = CaseStatus.INREVIEW;

        return this;
    }

    public Result<CaseEntity, Error> ApproveCase()
    {
        if (ReviewerId is null || CaseStatus != CaseStatus.INREVIEW)
            return new Error("Only case in status InRevew with assined reviewer can be approved", ErrorReason.InvalidOperation);

        CaseStatus = CaseStatus.APPROVED;

        return this;
    }

    public Result<CaseEntity, Error> RejectCase()
    {
        if (ReviewerId is null || CaseStatus != CaseStatus.INREVIEW)
            return new Error("Only case in status InRevew with assined reviewer can be rejected", ErrorReason.InvalidOperation);

        CaseStatus = CaseStatus.REJECTED;

        return this;
    }
}
