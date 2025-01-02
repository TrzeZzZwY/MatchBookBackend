using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetBook;
public class GetManyBooksHandler : IRequestHandler<GetManyBooksCommand, Result<PaginatedResult<GetBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyBooksHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetBookResult>, Error>> Handle(GetManyBooksCommand request, CancellationToken cancellationToken)
    {
        var books = _databaseContext.Books.AsQueryable();

        if (!request.ShowRemoved)
            books = books.Where(e => e.IsDeleted == false);

        if (request.Title is not null)
            books = books.Where(e => e.Title.ToLower().Contains(request.Title.ToLower()));

        if (request.InludeAuthorDetails)
            books = books.Include(e => e.Authors);

        if (request.AuthorId is not null)
            books = books.Where(e => e.Authors.Any(a => a.Id == request.AuthorId));

        var total = books.Count();
        books = books
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(
            books.ToList().Select(e => e.ToHandlerResult()).ToList(),
            total
            );
    }
}
