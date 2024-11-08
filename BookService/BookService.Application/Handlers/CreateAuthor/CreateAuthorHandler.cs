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

    public CreateAuthorHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
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

        return new CreateAuthorResult();
    }
}
