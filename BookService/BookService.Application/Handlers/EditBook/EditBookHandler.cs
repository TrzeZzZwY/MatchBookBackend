using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.EditBook;
public class EditBookHandler : IRequestHandler<EditBookCommand, Result<EditBookResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public EditBookHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<EditBookResult, Error>> Handle(EditBookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var book = await _databaseContext.Books.FindAsync([request.BookId], cancellationToken);
            if (book is null) return new Error($"Book is not found for id: {request.BookId}", ErrorReason.InternalError);

            book.Title = request.Title;

            await _databaseContext.Entry(book).Collection(e => e.Authors).LoadAsync();
            book.Authors.Clear();

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
            _databaseContext.Books.Update(book);
            await _databaseContext.SaveChangesAsync(cancellationToken);
            return new EditBookResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.BadRequest);
        }
    }
}
