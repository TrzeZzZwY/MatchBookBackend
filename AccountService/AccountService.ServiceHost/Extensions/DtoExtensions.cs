using AccountService.Application.Handlers.GetAdmin;
using AccountService.Application.Handlers.GetUser;
using AccountService.ServiceHost.Controllers.Dto.Admin;
using AccountService.ServiceHost.Controllers.Dto.RegisterUser;

namespace AccountService.ServiceHost.Extensions;

public static class DtoExtensions
{
    public static GetUserResponse ToDto(this GetUserResult result)
    {
        return new GetUserResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirtName = result.FirstName,
            LastName = result.LastName,
            BirthDate = result.BirthDate,
            Region = result.Region
        };
    }

    public static GetAdminResponse ToDto(this GetAdminResult result)
    {
        return new GetAdminResponse
        {
            Id = result.Id,
            Email = result.Email,
            FirtName = result.FirstName,
            LastName = result.LastName
        };
    }
}
