namespace MatchBook.Domain.Models;

public class Region
{
    public int Id { get; set; }

    public string RegionName { get; set; }

    public bool IsEnabled { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public List<BookPoint> BookPoints { get; set; }

    public List<ApplicationUser> Users { get; set; }
}
