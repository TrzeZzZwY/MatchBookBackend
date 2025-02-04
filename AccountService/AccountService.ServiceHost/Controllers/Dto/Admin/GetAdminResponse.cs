namespace AccountService.ServiceHost.Controllers.Dto.Admin;

public class GetAdminResponse
{
    public required int Id { get; set; }

    public required string Email { get; set; }

    public required string FirtName { get; set; }

    public required string LastName { get; set; }
}
