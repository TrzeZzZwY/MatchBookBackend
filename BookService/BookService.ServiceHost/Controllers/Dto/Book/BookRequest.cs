namespace BookService.ServiceHost.Controllers.Dto.Book;

public class BookRequest
{
    public string Title { get; init; }

    public List<int> AuthorsIds { get; init; }
}
