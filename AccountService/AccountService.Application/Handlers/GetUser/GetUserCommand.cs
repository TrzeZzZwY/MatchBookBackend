using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetUser;

public class GetUserCommand : IRequest<Result<GetUserResult?, Error>>
{
    public required int Id { get; set; }
}
