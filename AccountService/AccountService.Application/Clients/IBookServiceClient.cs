using AccountService.Domain.Common;
using CSharpFunctionalExtensions;

namespace AccountService.Application.Clients;

public interface IBookServiceClient
{
    public Task<Result<bool, Error>> UpdateItemsRegion();
}
