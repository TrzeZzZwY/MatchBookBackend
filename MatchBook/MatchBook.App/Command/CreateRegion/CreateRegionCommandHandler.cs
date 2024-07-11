using MatchBook.Domain.Models;
using MatchBook.Infrastructure;
using MediatR;

namespace MatchBook.App.Command.CreateRegion;

public class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand>
{
    private readonly RegionRepository _regionRepository;

    public CreateRegionCommandHandler(RegionRepository regionRepository)
    {
        _regionRepository = regionRepository;
    }
    public async Task Handle(CreateRegionCommand request, CancellationToken cancellationToken)
    {
        var region = new Region
        {
            RegionName = request.Name,
            IsEnabled = request.IsEnabled,
            CreateDate = DateTime.UtcNow
        };

        await _regionRepository.Add(region);
    }
}
