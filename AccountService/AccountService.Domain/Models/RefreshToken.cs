namespace AccountService.Domain.Models;
public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; }

    public DateTime ExpireDate { get; set; }

    public int AccountId { get; set; }

    public Account Account { get; set; }
}
