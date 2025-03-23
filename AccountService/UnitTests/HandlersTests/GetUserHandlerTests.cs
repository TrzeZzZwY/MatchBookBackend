using AccountService.Application.Handlers.GetAdmin;
using AccountService.Domain.Models;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using AccountService.Application.Handlers.GetUser;

namespace UnitTests.HandlersTests;

public class GetUserHandlerTests
{
    private IServiceProvider _serviceProvider;
    public GetUserHandlerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<DatabaseContext>(opt =>
        opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        serviceCollection.AddIdentity();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task WhenAccountIsNotFound_ThenNullIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var command = new GetUserCommand
        {
            Id = 55
        };

        var handler = new GetUserHandler(userManager, context);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
    
    [Fact]
    public async Task WhenAccountIsFound_ThenIsReturned()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        var userManager = services.GetRequiredService<UserManager<Account>>();
        var roleManager = services.GetRequiredService<RoleManager<AccountRole>>();

        var account = await userManager.SetupUserAccount(roleManager, context);
        var user = account.UserAccount;

        var command = new GetUserCommand
        {
            Id = user.Id
        };
        var expectedResult = new GetUserResult
        {
            Id = user.Id,
            Email = user.Account.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            Region = user.Region,
            Status = user.Account.Status
        };
        var handler = new GetUserHandler(userManager, context);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
    }
}
