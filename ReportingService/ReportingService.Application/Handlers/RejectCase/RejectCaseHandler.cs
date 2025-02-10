using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Application.Strategies;
using ReportingService.Domain.Common;
using ReportingService.Domain.Models;

namespace ReportingService.Application.Handlers.RejectCase;
public class RejectCaseHandler : IRequestHandler<RejectCaseCommand, Result<RejectCaseResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly IEnumerable<IStrategy<CaseEntity>> _strategies;

    public RejectCaseHandler(DatabaseContext databaseContext, IEnumerable<IStrategy<CaseEntity>> strategies)
    {
        _databaseContext = databaseContext;
        _strategies = strategies;
    }

    public async Task<Result<RejectCaseResult, Error>> Handle(RejectCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await _databaseContext.Cases.FindAsync([request.CaseId], cancellationToken);
        if (caseEntity is null) return new Error("Case not found", ErrorReason.NotFound);

        if (caseEntity.ReviewerId != request.ReviewerId)
            return new Error("Only assigned reviewer can reject case", ErrorReason.InvalidOperation);

        var rejectResult = caseEntity.RejectCase();
        if (rejectResult.IsFailure) return rejectResult.Error;

        await _databaseContext.SaveChangesAsync(cancellationToken);

        var strategy = _strategies.FirstOrDefault(e => e.CanHandle(caseEntity));
        if(strategy is not null)
        {
            await strategy.HandleAsync(caseEntity, cancellationToken);
        }

        return new RejectCaseResult();
    }
}
