namespace AccountService.Domain.Models;
public class AdminAccount
{
    public int Id { get; set; }

    public string FistName { get; set; }

    public string LastName { get; set; }

    public int? AccountCreatorId { get; set; }

    public AdminAccount? AccountCreator { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; }
}
