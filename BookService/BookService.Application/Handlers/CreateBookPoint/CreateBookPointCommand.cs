using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateBookPoint;
public class CreateBookPointCommand : IRequest<Result<CreateBookPointResult, Error>>
{
    public required int Lat { get; init; }

    public required int Long { get; init; }

    public required Region Region { get; init; }

    public int? Capacity { get; init; }
}
