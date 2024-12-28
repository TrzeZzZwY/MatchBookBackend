using BookService.Application.Extensions;
using BookService.Domain.Common;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.GetAuthor;
public class GetAuthorHandler : IRequestHandler<GetAuthorCommand?, Result<GetAuthorResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public GetAuthorHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<GetAuthorResult?, Error>> Handle(GetAuthorCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var author = await _databaseContext.Authors.FindAsync([request.AuthorId], cancellationToken);
            return author?.ToHandlerResult();
        }
        catch (Exception e)
        {
            return new Error(e.Message, ErrorReason.InternalError);
        }
    }
}
