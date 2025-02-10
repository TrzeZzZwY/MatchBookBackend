namespace BookService.Application.Handlers.GetMatches;
public class GetMatchesResult
{
    public required List<Match> Matches { get; set; }
}

public class Match
{
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public List<MatchBook> Items { get; set; }
}

public class MatchBook
{
    public BookItem OfferedBook { get; set; }
    public BookItem RequestedBook { get; set; }
}

public class BookItem
{
    public int UserBookItemId { get; set; }
    public string Title { get; set; }
    public int? ImageId { get; set; }
}

