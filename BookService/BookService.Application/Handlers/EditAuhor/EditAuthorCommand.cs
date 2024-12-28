using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditAuhor;
public class EditAuthorCommand : IRequest<Result<EditAuthorResult, Error>>
{
    public required int AuthorId { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Country { get; init; }

    public required int YearOfBirth { get; init; }
}
