using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.GetAdmin;

public class GetAdminCommand : IRequest<Result<GetAdminResult?, Error>>
{
    public required int Id { get; set; }
}
