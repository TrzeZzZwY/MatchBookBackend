using BookService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.DeleteBookPoint;
public class DeleteBookPointCommand : IRequest<Result<DeleteBookPointResult, Error>>
{
    public required int BookPointId { get; set; }
}
