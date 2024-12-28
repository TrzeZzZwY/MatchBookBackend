namespace BookService.ServiceHost.Controllers.Dto.Author;

public class AuthorResponse
{
    public required int Id { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public required string Country { get; init; }

    public required int YearOfBirth { get; init; }

    public required bool IsRemoved { get; init; }
}
