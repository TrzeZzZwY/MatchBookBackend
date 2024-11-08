using AccountService.Application.Handlers.RegisterUser;
using AccountService.Repository;
using AccountService.ServiceHost.Extensions;
using AccountService.ServiceHost.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add DB
var configuration = builder.Configuration;
var databaseOptions = configuration.GetSection("DatabaseOptions").Get<DatabaseOptions>()
    ?? throw new Exception("Cannot get Database options");

builder.Services.AddDbContext<DatabaseContext>(opt =>
    opt.UseSqlServer(databaseOptions.ConnectionString));

//Add CQRS
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(RegisterUserHandler).Assembly);
});

builder.Services.AddIdentity(configuration);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();