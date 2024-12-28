using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookService.Application.Handlers.GetBook;
public class GetManyBooksHandler : IRequestHandler<GetManyBooksCommand, Result<List<GetBookResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyBooksHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetBookResult>, Error>> Handle(GetManyBooksCommand request, CancellationToken cancellationToken)
    {
        var books = _databaseContext.Books.AsQueryable();

        if (!request.ShowRemoved)
            books = books.Where(e => e.IsDeleted == false);

        if (request.Title is not null)
            books = books.Where(e => e.Title.Contains(request.Title));

        if (request.InludeAuthorDetails)
            books = books.Include(e => e.Authors);

        books = books
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return books.ToList().Select(e => e.ToHandlerResult()).ToList();
    }
}
