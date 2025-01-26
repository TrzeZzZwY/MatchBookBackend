using AccountService.Domain.Common;
using AccountService.ServiceHost.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.ServiceHost.Extensions;

public static class ErrorExtensions
{
    public static ObjectResult ToErrorResult(this Error error)
    {
        var genericError = new GenericError
        {
            Description = error.Description
        };

        return error.Reason switch
        {
            ErrorReason.BadRequest => Result(genericError, StatusCodes.Status400BadRequest),
            ErrorReason.Unauthorized => Result(genericError, StatusCodes.Status401Unauthorized),
            _ => Result(genericError, StatusCodes.Status500InternalServerError),
        };
    }

    private static ObjectResult Result(GenericError error, int statusCode)
    => new ObjectResult(error) { StatusCode = statusCode };
}