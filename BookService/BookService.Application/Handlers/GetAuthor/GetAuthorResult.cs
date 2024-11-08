namespace BookService.Application.Handlers.GetAuthor;
public class GetAuthorResult
{
    public required int Id { get; init; }

    public required string FistName { get; init; }
    
    public required string LastName { get; init; }

    public required string Country { get; init; }

    public required int YearOfBirth { get; init; }
}
