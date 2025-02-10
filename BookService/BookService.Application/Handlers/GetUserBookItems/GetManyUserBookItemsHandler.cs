using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetUserBookItems;
public class GetManyUserBookItemsHandler : IRequestHandler<GetManyUserBookItemsCommand, Result<PaginatedResult<GetUserBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyUserBookItemsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetUserBookResult>, Error>> Handle(GetManyUserBookItemsCommand request, CancellationToken cancellationToken)
    {
        var userBooks = _databaseContext.UserBookItems.AsQueryable();

        if (request.Region is not null)
            userBooks = userBooks.Where(e => e.Region == request.Region);

        if (request.ItemStatus is not null)
            userBooks = userBooks.Where(e => e.Status == request.ItemStatus);

        if (request.UserId is not null)
            userBooks = userBooks.Where(e => e.UserId == request.UserId);

        if (request.Title is not null)
        {
            userBooks.Include(e => e.BookReference);
            userBooks = userBooks.Where(e => e.BookReference.Title.ToLower().Contains(request.Title.ToLower()));
        }

        if (request.StartDate is not null)
            userBooks = userBooks.Where(e => e.CreateDate >= request.StartDate);

        if (request.EndDate is not null)
            userBooks = userBooks.Where(e => e.CreateDate <= request.EndDate);

        userBooks = userBooks.OrderBy(e => e.CreateDate);
        var total = userBooks.Count();
        userBooks = userBooks
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        if (request.InludeAuthorDetails)
            userBooks = userBooks.Include(e => e.BookReference).ThenInclude(e => e.Authors);
        else
            userBooks = userBooks.Include(e => e.BookReference);

        return request.PaginationOptions.ToPaginatedResult(
            userBooks.ToList().Select(e => e.ToHandlerResult()).ToList(),
            total
            );
    }
}
