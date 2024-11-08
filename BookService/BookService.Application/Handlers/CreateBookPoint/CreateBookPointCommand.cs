using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateBookPoint;
public class CreateBookPointCommand : IRequest<Result<CreateBookPointResult, Error>>
{
    public required int Lat { get; set; }

    public required int Long { get; set; }

    public required Region Region { get; set; }

    public int? Capacity { get; set; }
}
