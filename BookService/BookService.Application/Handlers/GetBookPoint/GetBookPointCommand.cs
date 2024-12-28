using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBookPoint;
public class GetBookPointCommand : IRequest<Result<GetBookPointResult?, Error>>
{
    public required int BookPointId { get; set; }
}
