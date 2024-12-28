using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateAuthor;
public class CreateAuthorCommand : IRequest<Result<CreateAuthorResult, Error>>
{
    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Country { get; init; }

    public required int YearOfBirth { get; init; }
}
