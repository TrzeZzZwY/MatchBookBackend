using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetBook;
public class GetBookHandler : IRequestHandler<GetBookCommand, Result<GetBookResult?, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetBookHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetBookResult?, Error>> Handle(GetBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _databaseContext.Books.FindAsync([request.BookId], cancellationToken);

        if (book is not null)
        {
            if (request.InludeAuthorDetails)
                await _databaseContext.Entry(book).Collection(e => e.Authors).LoadAsync(cancellationToken);
        }

        return book?.ToHandlerResult();
    }
}
