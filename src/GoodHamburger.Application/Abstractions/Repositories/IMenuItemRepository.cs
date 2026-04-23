using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Repositories;

public interface IMenuItemRepository
{
    Task<List<MenuItem>> ListAsync(CancellationToken cancellationToken);
    Task<List<MenuItem>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken);
}