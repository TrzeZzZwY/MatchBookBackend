using BookService.Application.Clients;
using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.CreateAuthor;
public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, Result<CreateAuthorResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly ReportingServiceClient _reportingServiceClient;
    public CreateAuthorHandler(DatabaseContext databaseContext, ReportingServiceClient reportingServiceClient)
    {
        _databaseContext = databaseContext;
        _reportingServiceClient = reportingServiceClient;
    }
    public async Task<Result<CreateAuthorResult, Error>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _databaseContext.Authors
            .Where(e =>
            e.FirstName == request.FirstName &&
            e.LastName == request.LastName &&
            e.YearOfBirth == request.YearOfBirth &&
            e.Country == request.Country).FirstOrDefaultAsync(cancellationToken);

        if (author is not null) return new Error("Author already exist", ErrorReason.BadRequest);

        author = new Author
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            YearOfBirth = request.YearOfBirth,
            Country = request.Country
        };

        await _databaseContext.AddAsync(author, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        var createCaseResult = await _reportingServiceClient.CreateAuthorCase(author);
        if (createCaseResult.IsFailure) return createCaseResult.Error;

        return new CreateAuthorResult { AuthorId = author.Id };
    }
}
