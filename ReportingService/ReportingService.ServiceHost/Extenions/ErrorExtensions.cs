using Microsoft.AspNetCore.Mvc;
using ReportingService.Domain.Common;
using ReportingService.ServiceHost.Controllers.Dto;

namespace ReportingService.ServiceHost.Extenions;

public static class ErrorExtensions
{
    public static ObjectResult ToErrorResult(this Error error)
    {
        var genericError = new GenericError
        {
            Description = error.Description
        };

        return new ObjectResult(genericError) { StatusCode = ResolveStatusCode(error.Reason) };
    }

    private static int ResolveStatusCode(ErrorReason reason)
    {
        return reason switch
        {
            ErrorReason.BadRequest => StatusCodes.Status400BadRequest,
            ErrorReason.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorReason.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError,
        };
    }
}