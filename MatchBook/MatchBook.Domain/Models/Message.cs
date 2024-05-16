namespace MatchBook.Domain.Models;

public class Message
{
    public int Id { get; set; }

    public string messageData { get; set; }

    public int UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int ChatId { get; set; }

    public Chat Chat { get; set; }
}
