using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetUserBookItemHandler : IRequestHandler<GetUserBookItemCommand, Result<GetUserBookResult?, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetUserBookItemHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetUserBookResult?, Error>> Handle(GetUserBookItemCommand request, CancellationToken cancellationToken)
    {
        var item = await _databaseContext.UserBookItems.FindAsync([request.UserBookItemId], cancellationToken);
        return item?.ToHandlerResult();
    }
}
