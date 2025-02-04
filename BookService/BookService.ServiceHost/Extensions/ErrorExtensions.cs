using BookService.Domain.Common;
using BookService.ServiceHost.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BookService.ServiceHost.Extensions;

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
            ErrorReason.BadRequest => new ObjectResult(genericError){StatusCode = StatusCodes.Status400BadRequest},
            ErrorReason.Unauthorized => new ObjectResult(genericError) { StatusCode = StatusCodes.Status401Unauthorized },
            _ => new ObjectResult(genericError) { StatusCode = StatusCodes.Status500InternalServerError },
        };
    }
}