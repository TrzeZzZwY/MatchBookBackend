using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetManyAuthorsCommand : IRequest<Result<PaginatedResult<GetAuthorResult>,Error>>
{
    public required PaginationOptions PaginationOptions { get; init; }

    public bool ShowRemoved { get; set; } = false;

    public string? AuthorName { get; set; } = null;
}
