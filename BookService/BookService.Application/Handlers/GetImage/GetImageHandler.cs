using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetImage;
public class GetImageHandler : IRequestHandler<GetImageCommand, Result<GetImageResult?, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetImageHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetImageResult?, Error>> Handle(GetImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var image = await _databaseContext.Images.FindAsync([request.ImageId], cancellationToken);
            if (image is null) return (GetImageResult)null;

            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var path = Path.Combine(parentDirectory, "Images", image.GetFileName());

            var bytes = File.ReadAllBytes(path);


            return new GetImageResult
            {
                ImageType = image.ImageType,
                ImageBytes = bytes
            };
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
