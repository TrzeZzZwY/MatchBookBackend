using System.Security.Claims;
using AccountService.Application.Handlers.CreateToken;
using AccountService.Application.Services;
using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests.HandlersTests;

public class CreateTokenHandlerTests
{
    private IServiceProvider _serviceProvider;
    private Mock<ITokenService> _tokenService;
    public CreateTokenHandlerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<DatabaseContext>(opt =>
        opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        serviceCollection.AddIdentity();
        _serviceProvider = serviceCollection.BuildServiceProvider();

    }

    [Fact]
    public async Task WhenAccountIsNotFound_ThenErrorIsReturned()
    {
        _tokenService = new Mock<ITokenService>(); 
        var command = new CreateTokenCommand
        {
            AccountId = 1,
            UserId = 1,
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var handelr = new CreateTokenHandler(context, _tokenService.Object, userManager);
        var result = await handelr.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("User not found");
        result.Error.Reason.Should().Be(ErrorReason.NotFound);
    }

    [Fact]
    public async Task WhenGenerateJWTReturnError_ThenErrorIsReturned()
    {
        _tokenService = new Mock<ITokenService>();
        _tokenService.Setup(e => e.GenerateJsonWebToken(It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync(new Error("GenerateJsonWebTokenFail", ErrorReason.InternalError));

        var command = new CreateTokenCommand
        {
            AccountId = 1,
            UserId = 1,
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        await userManager.SetupAdminAccount(roleManager, context);

        var handelr = new CreateTokenHandler(context, _tokenService.Object, userManager);
        var result = await handelr.Handle(command, new CancellationToken());

        result.IsFailure.Should().BeTrue();
        result.Error.Description.Should().Be("GenerateJsonWebTokenFail");
        result.Error.Reason.Should().Be(ErrorReason.InternalError);
    }

    [Fact]
    public async Task WhenRefreshTokenIsNull_ThenNewIsGenerated()
    {
        var jwt = "sadasdas.watrwqeqd.hfedgwerr";
        var refreshToken = "dawiasdasdjaspeaper";
        _tokenService = new Mock<ITokenService>();
        _tokenService.Setup(e => e.GenerateJsonWebToken(It.IsAny<IEnumerable<Claim>>()))
            .ReturnsAsync(jwt);
        _tokenService.Setup(e => e.GenerateRefreshToken())
            .Returns(refreshToken);

        var command = new CreateTokenCommand
        {
            AccountId = 1,
            UserId = 1,
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        await userManager.SetupAdminAccount(roleManager, context);

        var handelr = new CreateTokenHandler(context, _tokenService.Object, userManager);
        var result = await handelr.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
        result.Value.RefreshToken.Should().Be(refreshToken);
        result.Value.Token.Should().Be(jwt);
    }
}
