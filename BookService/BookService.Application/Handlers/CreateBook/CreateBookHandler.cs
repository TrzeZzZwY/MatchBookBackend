using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateBook;
public class CreateBookHandler : IRequestHandler<CreateBookCommand, Result<CreateBookResult, Error>>
{

    private readonly DatabaseContext _databaseContext;

    public CreateBookHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateBookResult, Error>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book { Title = request.Title };

        if (request.AuthorsId is not null)
        {
            List<Author> authors = new List<Author>();
            foreach (var authorId in request.AuthorsId)
            {
                var author = await _databaseContext.Authors.FindAsync(authorId);
                if (author is null) return new Error("Cannot find requested authors", ErrorReason.BadRequest);
                authors.Add(author);
            }

            book.Authors = authors;
        }

        await _databaseContext.Books.AddAsync(book, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);
        return new CreateBookResult { BookId = book.Id };
    }
}
