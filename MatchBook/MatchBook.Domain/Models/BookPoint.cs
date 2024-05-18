namespace MatchBook.Domain.Models;

public class BookPoint
{
    public int Id { get; set; }

    public int Lat { get; set; }

    public int Long { get; set; }

    public int? RegionId { get; set; }

    public Region? Region { get; set; }

    public int? Capacity { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public List<Book> Books { get; set; }
}

