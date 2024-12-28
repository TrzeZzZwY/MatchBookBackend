using BookService.Application.Handlers.GetAuthor;

namespace BookService.Application.Handlers.GetBook;
public class GetBookResult
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public List<GetAuthorResult>? Authors { get; init; }

    public required bool IsRemoved { get; init; }
}
