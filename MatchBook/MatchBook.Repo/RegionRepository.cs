using MatchBook.Domain.Models;
using MatchBook.Infrastructure.Data;

namespace MatchBook.Infrastructure;

public class RegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task Add(Region region)
    {
        await _context.AddAsync(region);
        await _context.SaveChangesAsync();
    }
}
