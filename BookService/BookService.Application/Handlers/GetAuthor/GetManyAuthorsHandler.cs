using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetManyAuthorsHandler : IRequestHandler<GetManyAuthorsCommand, Result<PaginatedResult<GetAuthorResult>, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetManyAuthorsHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<PaginatedResult<GetAuthorResult>, Error>> Handle(GetManyAuthorsCommand request, CancellationToken cancellationToken)
    {
        var authors = _databaseContext.Authors.AsQueryable();

        if (!request.ShowRemoved)
            authors = authors.Where(e => e.IsDeleted == false);

        if (request.AuthorName is not null)
        {
            var authorFilters = request.AuthorName.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (authorFilters.Length != 0)
                authors = authors.Where(auth => authorFilters.All(f => auth.FirstName.Contains(f) || auth.LastName.Contains(f)));

        }

        var total = authors.Count();
        authors = authors
            .Skip((request.PaginationOptions.PageNumber - 1) * request.PaginationOptions.PageSize)
            .Take(request.PaginationOptions.PageSize);

        return request.PaginationOptions.ToPaginatedResult(
            authors.ToList().Select(e => e.ToHandlerResult()).ToList(),
            total
            );
    }
}
