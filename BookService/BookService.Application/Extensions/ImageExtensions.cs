using BookService.Domain.Common;
using BookService.Domain.Models;

namespace BookService.Application.Extensions;
public static class ImageExtensions
{
    public static ImageType ToImageType(this string contentType)
    {
        return contentType switch
        {
            "image/jpeg" => ImageType.JPEG,
            "image/png" => ImageType.PNG,
            "image/heic" => ImageType.PNG,
            _ => ImageType.UNKNOWN
        };
    }

    public static string ToContentType(this ImageType imageType)
    {
        return imageType switch
        {
            ImageType.JPEG => "image/jpeg" ,
            ImageType.PNG => "image/png" ,
            _ => throw new ArgumentException()
        };
    }

    public static string GetFileName(this Image image)
    {
        return string.Concat(image.Title, "_", image.CreateDate.ToString("yyyy_dd_M_HH_mm_ss"), image.ImageExtension);
    }
}
