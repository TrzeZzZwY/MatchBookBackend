using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetImage;
public class GetImageCommand : IRequest<Result<GetImageResult?, Error>>
{
    public int ImageId { get; set; }
}
