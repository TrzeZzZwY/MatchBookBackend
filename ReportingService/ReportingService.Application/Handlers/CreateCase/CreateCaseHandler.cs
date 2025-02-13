using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using ReportingService.Domain.Common;
using ReportingService.Domain.Models;
using System.Text.Json;

namespace ReportingService.Application.Handlers.CreateCase;
public class CreateCaseHandler : IRequestHandler<CreateCaseCommand, Result<CreateCaseResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public CreateCaseHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateCaseResult, Error>> Handle(CreateCaseCommand request, CancellationToken cancellationToken)
    {
        var serializedValues = JsonSerializer.Serialize(request.CaseFields);

        var entity = new CaseEntity(
            userId: request.UserId,
            caseItemType: request.Type,
            itemId: request.ItemId,
            serializedCaseFields: serializedValues,
            reportNote: request.Notes,
            reportType: request.ReportType
            );

        await _databaseContext.AddAsync(entity, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new CreateCaseResult
        {
            CaseId = entity.Id
        };
    }
}
