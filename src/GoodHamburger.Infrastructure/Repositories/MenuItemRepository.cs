using GoodHamburger.Application.Abstractions.Repositories;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public sealed class MenuItemRepository(AppDbContext dbContext) : IMenuItemRepository
{
    public Task<List<MenuItem>> ListAsync(CancellationToken cancellationToken)
    {
        return dbContext.MenuItems
            .AsNoTracking()
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<List<MenuItem>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken)
    {
        return dbContext.MenuItems
            .Where(x => ids.Contains(x.Id))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}