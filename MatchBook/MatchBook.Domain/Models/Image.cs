using MatchBook.Domain.Models.Identity;

namespace MatchBook.Domain.Models;

public class Image
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public int TargetId { get; set; }

    public ImageTarget Target { get; set; }

    public int? BookId { get; set; }
    public Book? Book { get; set; }

    public int? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
