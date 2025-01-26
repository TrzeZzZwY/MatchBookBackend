using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.DecodeToken;
public class DecodeTokenCommand : IRequest<Result<DecodeResult, Error>>
{
    public required string Token { get; set; }
}
