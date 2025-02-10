using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.ApproveCase;
public class ApproveCaseHandler : IRequestHandler<ApproveCaseCommand, Result<ApproveCaseResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public ApproveCaseHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<ApproveCaseResult, Error>> Handle(ApproveCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await _databaseContext.Cases.FindAsync([request.CaseId], cancellationToken);
        if (caseEntity is null) return new Error("Case not found", ErrorReason.NotFound);

        if (caseEntity.ReviewerId != request.ReviewerId)
            return new Error("Only assigned reviewer can approve case", ErrorReason.InvalidOperation);

        var approveResult = caseEntity.ApproveCase();
        if (approveResult.IsFailure) return approveResult.Error;

        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new ApproveCaseResult();
    }
}