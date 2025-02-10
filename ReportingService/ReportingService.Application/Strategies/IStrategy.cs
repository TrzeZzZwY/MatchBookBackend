using CSharpFunctionalExtensions;
using ReportingService.Domain.Common;

namespace ReportingService.Application.Strategies;
public interface IStrategy<T>
{
    bool CanHandle(T item);

    Task<Result<bool, Error>> HandleAsync(T item, CancellationToken cancellation);
}
