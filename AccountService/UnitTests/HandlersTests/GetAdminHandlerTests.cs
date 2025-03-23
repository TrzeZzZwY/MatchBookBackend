using AccountService.Application.Handlers.GetAdmin;
using AccountService.Domain.Models;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;

namespace UnitTests.HandlersTests;

public class GetAdminHandlerTests
{
    private IServiceProvider _serviceProvider;
    public GetAdminHandlerTests()
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

        var command = new GetAdminCommand
        {
            Id = 55
        };

        var handler = new GetAdminHandler(userManager, context);
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

        var account = await userManager.SetupAdminAccount(roleManager, context);
        var admin = account.AdminAccount;

        var command = new GetAdminCommand
        {
            Id = admin.Id
        };
        var expectedResult = new GetAdminResult
        {
            Id = admin.Id,
            Email = admin.Account.Email!,
            FirstName = admin.FirstName,
            LastName = admin.LastName,
            AccountCreatorId = admin.AccountCreatorId
        };
        var handler = new GetAdminHandler(userManager, context);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResult);
    }
}
