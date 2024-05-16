using Microsoft.AspNetCore.Identity;

namespace MatchBook.Domain.Models;

public class ApplicationUser : IdentityUser<int>
{
    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public int RegionId { get; set; }

    public Region Region { get; set; }

    public List<Book> UserBooks { get; set; }

    public List<Author> FollowedAuthors { get; set; }

    public List<BookTitle> FollowedTitles { get; set; }

    public List<Chat> Chats { get; set; }

    public List<Book> Likes { get; set; }

    public List<Message> Messages { get; set; }
}
