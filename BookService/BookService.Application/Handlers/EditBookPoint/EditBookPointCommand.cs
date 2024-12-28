using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditBookPoint;
public class EditBookPointCommand : IRequest<Result<EditBookPointResult, Error>>
{
    public required int BookPointId { get; init; }

    public required int Lat { get; init; }

    public required int Long { get; init; }

    public required Region Region { get; init; }

    public int? Capacity { get; init; }
}