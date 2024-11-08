namespace BookService.Domain.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; }

    public List<Author> Authors { get; set; }

    public List<UserBookItem> BookItems { get; set; }
}
