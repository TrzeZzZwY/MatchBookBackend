using MatchBook.Domain.Enums;

namespace MatchBook.Domain.Models;

public class ImageTarget
{
    public int Id { get; set; }

    public ImageType Type { get; set; }

    public string Path { get; set; }

    public int ImageId { get; set; }

    public Image Image { get; set; }
}
