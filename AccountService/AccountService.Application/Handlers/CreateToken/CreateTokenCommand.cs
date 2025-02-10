using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.CreateToken;
public class CreateTokenCommand: IRequest<Result<CreateTokenResult, Error>>
{
    public required int AccountId { get; set; }

    public required int UserId { get; set; }
}
