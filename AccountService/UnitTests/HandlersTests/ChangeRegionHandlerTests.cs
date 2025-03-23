using AccountService.Application.Clients;
using AccountService.Application.Handlers.ChangeRegion;
using AccountService.Domain.Common;
using AccountService.Domain.Models;
using AccountService.Repository;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace UnitTests.HandlersTests;

public class ChangeRegionHandlerTests
{
    private Mock<IBookServiceClient> _bookServiceClient;
    private IServiceProvider _serviceProvider;

    public ChangeRegionHandlerTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContext<DatabaseContext>(opt =>
        opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    [Fact]
    public async Task WhenUserIsNotFound_ThenErrorIsReturned()
    {
        _bookServiceClient = new Mock<IBookServiceClient>();
        var command = new ChangeUserRegionCommand
        {
            region = Region.WARSAW,
            UserId = 555
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DatabaseContext>();

        var handler = new ChanageUserRegionHandler(context, _bookServiceClient.Object);
        var result = await handler.Handle(command, new CancellationToken());

        result.Error.Should().NotBeNull();
        result.Error.Reason.Should().Be(ErrorReason.NotFound);
        result.Error.Description.Should().Be("User not found");
    }

    [Fact]
    public async Task WhenUserIsPresentIndatabase_ThenRegionIsChanged()
    {
        int userId = 3;
        _bookServiceClient = new Mock<IBookServiceClient>();
        var command = new ChangeUserRegionCommand
        {
            region = Region.WARSAW,
            UserId = userId
        };

        await using var scope = _serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<DatabaseContext>();

        await SetupDatabase(context, userId);

        var handler = new ChanageUserRegionHandler(context, _bookServiceClient.Object);
        var result = await handler.Handle(command, new CancellationToken());

        result.IsSuccess.Should().BeTrue();
    }

    private async Task SetupDatabase(DatabaseContext context, int userId)
    {
        context.UserAccounts.Add(new UserAccount { Id = userId, FirstName = "John", LastName = "Tester"});
        await context.SaveChangesAsync();
    }
}
