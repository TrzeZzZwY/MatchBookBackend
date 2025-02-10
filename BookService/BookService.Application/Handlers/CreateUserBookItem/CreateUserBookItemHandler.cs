using BookService.Application.Clients;
using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateUserBookItem;
public class CreateUserBookItemHandler : IRequestHandler<CreateUserBookItemCommand, Result<CreateUserBookItemResult, Error>>
{
    private readonly DatabaseContext _databaseContext;
    private readonly AccountServiceClient _accountServiceClient;
    public CreateUserBookItemHandler(DatabaseContext databaseContext, AccountServiceClient accountServiceClient)
    {
        _databaseContext = databaseContext;
        _accountServiceClient = accountServiceClient;
    }

    public async Task<Result<CreateUserBookItemResult, Error>> Handle(CreateUserBookItemCommand request, CancellationToken cancellationToken)
    {
        var bookReference = await _databaseContext.Books.FindAsync([request.BookReferenceId], cancellationToken);
        if (bookReference is null) return new Error($"Book not found for bookReferenceId: {request.BookReferenceId}", ErrorReason.BadRequest);

        var imageReference = await _databaseContext.Images.FindAsync([request.ImageId], cancellationToken);

        var user = await _accountServiceClient.FetchUser(request.UserId);
        if (user.IsFailure) return user.Error;

        var userBookItem = new UserBookItem
        {
            UserId = request.UserId,
            Description = request.Description,
            CreateDate = DateTime.UtcNow,
            BookReferenceId = request.BookReferenceId,
            ItemImage = imageReference,
            Region = user.Value.Region
        };

        if (request.BookPointId != null)
            userBookItem.BookPointId = request.BookPointId;

        var updateStatusResult = userBookItem.UpdateStatus(request.Status);
        if (updateStatusResult.IsFailure) return updateStatusResult.Error;

        await _databaseContext.AddAsync(userBookItem, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new CreateUserBookItemResult { UserBookItemId = userBookItem.Id };
    }
}
