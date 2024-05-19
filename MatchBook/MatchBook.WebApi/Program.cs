using MatchBook.App.Command.CreateUser;
using MatchBook.Domain.Models;
using MatchBook.Infrastructure;
using MatchBook.Infrastructure.Data;
using MatchBook.WebApi.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var databaseOptions = configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(databaseOptions.ConnectionString));

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddApiEndpoints();

builder.Services.AddScoped<RegionRepository>();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(CreateUserCommandHandler).Assembly);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGroup("/identity").MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();
