using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetManyUserBookItemsHandler : IRequestHandler<GetManyUserBookItemsCommand, Result<List<GetUserBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyUserBookItemsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetUserBookResult>, Error>> Handle(GetManyUserBookItemsCommand request, CancellationToken cancellationToken)
    {
        var userBooks = _databaseContext.UserBookItems.AsQueryable();

        userBooks = userBooks
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        userBooks = userBooks.Include(e => e.BookReference);

        return userBooks.ToList().Select(e => e.ToHandlerResult()).ToList();
    }
}
