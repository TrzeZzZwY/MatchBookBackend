using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;

namespace AccountService.Application.Handlers.ChangeRegion;
public class ChangeUserRegionCommand : IRequest<Result<ChangeUserRegionResult, Error>>
{
    public int UserId { get; init; }

    public Region region { get; init; }
}
