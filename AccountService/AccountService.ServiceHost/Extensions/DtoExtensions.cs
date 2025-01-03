﻿using AccountService.Application.Handlers.GetUser;
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
            BirthDate = result.BirthDate
        };
    }
}
