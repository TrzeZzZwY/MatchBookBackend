using BookService.Domain.Common;

namespace BookService.Application.Handlers.GetImage;
public class GetImageResult
{
    public required ImageType ImageType { get; set; }
    public required byte[] ImageBytes { get; set; }
}
