using MatchBook.Domain.Models.Identity;

namespace MatchBook.Domain.Models;

public class BookTitle
{
    public int Id { get; set; }

    public string Title { get; set; }

    public List<Book> Books { get; set; }

    public List<ApplicationUser> Followers { get; set; }
}

