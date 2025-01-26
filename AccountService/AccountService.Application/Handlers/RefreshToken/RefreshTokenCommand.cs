using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.RefreshToken;
public class RefreshTokenCommand : IRequest<Result<RefreshTokenResult, Error>>
{
    public required string ExpiredToken { get; init; }

    public required string RefreshToken { get; init; }
}