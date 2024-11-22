namespace AccountService.ServiceHost.Controllers.Dto.RegisterUser;

public class GetUserResponse
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string FirtName { get; set; }

    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }
}
