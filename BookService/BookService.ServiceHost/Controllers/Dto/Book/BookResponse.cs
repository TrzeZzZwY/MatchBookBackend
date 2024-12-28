using BookService.ServiceHost.Controllers.Dto.Author;

namespace BookService.ServiceHost.Controllers.Dto.Book;

public class BookResponse
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public IEnumerable<AuthorResponse>? Authors { get; init; }

    public required bool IsRemoved { get; init; }
}
