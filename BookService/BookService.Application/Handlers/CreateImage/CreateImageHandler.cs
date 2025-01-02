using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateImage;
public class CreateImageHandler : IRequestHandler<CreateImageCommand, Result<CreateImageResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public CreateImageHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateImageResult, Error>> Handle(CreateImageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var extension = Path.GetExtension(request.FileName);
            var title = Path.GetFileNameWithoutExtension(request.FileName);

            var image = new Image
            {
                Title = title,
                ImageType = request.ImageType,
                ImageExtension = extension,
                CreateDate = DateTime.UtcNow
            };

            var entity = await _databaseContext.Images.AddAsync(image, cancellationToken);
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var path = Path.Combine(parentDirectory, "Images", image.GetFileName());

            using var ms = new MemoryStream(request.ByteArray);
            using var fs = new FileStream(path, FileMode.OpenOrCreate);
            ms.WriteTo(fs);

            await _databaseContext.SaveChangesAsync(cancellationToken);

            return new CreateImageResult { ImageId = image.Id };
        }
        catch(Exception e)
        {
            return new Error(e.Message + "\n" + e.InnerException, ErrorReason.InternalError);
        }
    }
}
