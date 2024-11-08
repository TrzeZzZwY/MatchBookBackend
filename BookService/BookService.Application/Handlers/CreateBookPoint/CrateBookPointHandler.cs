using BookService.Domain.Common;
using BookService.Domain.Models;
using BookService.Repository;
using CSharpFunctionalExtensions;
using MediatR;

namespace BookService.Application.Handlers.CreateBookPoint;
public class CrateBookPointHandler : IRequestHandler<CreateBookPointCommand, Result<CreateBookPointResult, Error>>
{
    private readonly DatabaseContext _databaseContext;

    public CrateBookPointHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Result<CreateBookPointResult, Error>> Handle(CreateBookPointCommand request, CancellationToken cancellationToken)
    {
        var bookPoint = _databaseContext.BookPoints.Where(e => e.Lat == request.Lat && e.Long == request.Long).FirstOrDefault();

        if (bookPoint is not null) 
            return new Error($"BookPoint for given Lat: {request.Lat} and Long: {request.Long} already exist", ErrorReason.BadRequest);

        bookPoint = new BookPoint
        {
            Lat = request.Lat,
            Long = request.Long,
            Region = request.Region,
            Capacity = request.Capacity,
            CreateDate = DateTime.UtcNow
        };

        await _databaseContext.AddAsync(bookPoint, cancellationToken);
        await _databaseContext.SaveChangesAsync(cancellationToken);

        return new CreateBookPointResult();    
    }
}
