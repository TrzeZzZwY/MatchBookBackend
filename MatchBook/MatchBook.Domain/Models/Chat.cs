namespace MatchBook.Domain.Models;

public class Chat
{
    public int Id { get; set; }

    public string Topic { get; set; }

    public List<ApplicationUser> Members { get; set; }

    public List<Message> Messages { get; set; }
}

