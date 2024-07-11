using MatchBook.Domain.Models.Identity;

namespace MatchBook.Domain.Models;

public class Author
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Surename { get; set; }

    public List<Book> Books { get; set; }

    public List<ApplicationUser> Followers { get; set; }
}

