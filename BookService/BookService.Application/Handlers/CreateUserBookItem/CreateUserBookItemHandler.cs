using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateUserBookItem;
public class CreateUserBookItemHandler : IRequestHandler<CreateUserBookItemCommand, Result<CreateUserBookItemResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public CreateUserBookItemHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateUserBookItemResult, Error>> Handle(CreateUserBookItemCommand request, CancellationToken cancellationToken)
    {

        var userBookItem = new UserBookItem
        {
            UserId = request.UserId,
            Description = request.Description,
            CreateDate = DateTime.UtcNow,
            BookReferenceId = request.BookReferenceId
        };

        if (request.BookPointId != null)
            userBookItem.BookPointId = request.BookPointId;

        var updateStatusResult = userBookItem.UpdateStatus(request.Status);
        if (updateStatusResult.IsFailure) return updateStatusResult.Error;

        await _databaseContext.AddAsync(userBookItem, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new CreateUserBookItemResult();
    }
}
