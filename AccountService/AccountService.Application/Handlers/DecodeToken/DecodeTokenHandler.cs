using AccountService.Application.Services;
using AccountService.Domain.Common;
using CSharpFunctionalExtensions;
using MediatR;
using System.Security.Claims;

namespace AccountService.Application.Handlers.DecodeToken;
public class DecodeTokenHandler : IRequestHandler<DecodeTokenCommand, Result<DecodeResult, Error>>
{
    private readonly ITokenService _tokenService;

    public DecodeTokenHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    public async Task<Result<DecodeResult, Error>> Handle(DecodeTokenCommand request, CancellationToken cancellationToken)
    {
        var validateResult = await _tokenService.ValidateToken(request.Token);
        if (validateResult.IsFailure) return validateResult.Error;

        var readTokenResult = await _tokenService.ReadToken(request.Token);
        if (readTokenResult.IsFailure) return readTokenResult.Error;

        var jwt = readTokenResult.Value;

        var region = Enum.Parse<Region>(jwt.Claims.FirstOrDefault(e => e.Type == "UserRegion").Value);
        var userId = int.Parse(jwt.Claims.FirstOrDefault(e => e.Type == ClaimTypes.NameIdentifier).Value);
        var roles = jwt.Claims.Where(e => e.Type == ClaimTypes.Role).Select(e => e.Value).ToList();
        
        return new DecodeResult
        {
            Region = region,
            UserId = userId,
            Roles = roles,
            ValidTo = jwt.ValidTo,
            ValidFrom = jwt.ValidFrom
        };
    }
}
