using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetManyAuthorsHandler : IRequestHandler<GetManyAuthorsCommand, Result<List<GetAuthorResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyAuthorsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<List<GetAuthorResult>, Error>> Handle(GetManyAuthorsCommand request, CancellationToken cancellationToken)
    {
        var authors = _databaseContext.Authors.AsQueryable();

        authors = authors
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return authors.ToList().Select(e => e.ToHandlerResult()).ToList();
    }
}
