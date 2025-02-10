using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Handlers.AssignToCase;
public class AssingToCaseHandler : IRequestHandler<AssingToCaseCommand, Result<AssingToCaseResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public AssingToCaseHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<AssingToCaseResult, Error>> Handle(AssingToCaseCommand request, CancellationToken cancellationToken)
    {
        var caseEntity = await _databaseContext.Cases.FindAsync([request.CaseId], cancellationToken);
        if (caseEntity is null) return new Error("Case not found", ErrorReason.NotFound);

        var assingResult = caseEntity.AssignReviewer(reviewerId: request.ReviewerId);
        if (assingResult.IsFailure) return assingResult.Error;

        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new AssingToCaseResult();
    }
}