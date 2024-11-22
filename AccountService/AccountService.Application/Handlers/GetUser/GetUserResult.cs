namespace AccountService.Application.Handlers.GetUser;

public record GetUserResult
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }
}
