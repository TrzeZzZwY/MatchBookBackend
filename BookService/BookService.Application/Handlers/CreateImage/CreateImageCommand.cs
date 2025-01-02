using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateImage;
public class CreateImageCommand : IRequest<Result<CreateImageResult, Error>>
{
    public required string FileName { get; set; }

    public required ImageType ImageType { get; set; }

    public required byte[] ByteArray { get; set; }
}
