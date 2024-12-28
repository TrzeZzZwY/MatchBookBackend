using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditAuhor;
public class EditAuthorHandler : IRequestHandler<EditAuthorCommand, Result<EditAuthorResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditAuthorHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditAuthorResult, Error>> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _databaseContext.Authors.FindAsync([request.AuthorId], cancellationToken);
            if (author is null) return new Error($"Author not found for id: {request.AuthorId}", ErrorReason.BadRequest);

            author.FirstName = request.FirstName;
            author.LastName = request.LastName;
            author.YearOfBirth = request.YearOfBirth;
            author.Country = request.Country;

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return new EditAuthorResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
