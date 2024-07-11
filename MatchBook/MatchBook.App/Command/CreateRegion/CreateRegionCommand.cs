using MediatR;

namespace MatchBook.App.Command.CreateRegion;

public class CreateRegionCommand : IRequest
{
    public required string Name { get; init; }

    public bool IsEnabled { get; init; }
}
