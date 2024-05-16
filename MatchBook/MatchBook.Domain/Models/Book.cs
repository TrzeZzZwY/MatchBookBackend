using MatchBook.Domain.Enums;

namespace MatchBook.Domain.Models;

public class Book
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int TitleId { get; set; }

    public BookTitle Title { get; set; }

    public string Category { get; set; }

    public int BookPointId { get; set; }

    public BookPoint BookPoint { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public string Description { get; set; }

    public int? ImageId { get; set; }

    public Image? Image { get; set; }

    public int Views { get; set; }

    public BookVisibility Visibility { get; set; }

    public List<Author> Authors { get; set; }

    public List<ApplicationUser> Likes { get; set; }
}
