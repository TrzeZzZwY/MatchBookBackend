using BookService.Domain.Common;

namespace BookService.Domain.Models;
public class Image
{
    public int Id { get; set; }

    public string Title { get; set; }

    public ImageType ImageType { get; set; }

    public string ImageExtension { get; set; }

    public DateTime CreateDate { get; set; }

    public int? UserBookItemId { get; set; }

    public UserBookItem? UserBookItem { get; set; }
}