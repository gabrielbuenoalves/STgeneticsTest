using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.Abstractions.Repositories;

public interface IOrderRepository
{
    Task<List<Order>> ListAsync(CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Order order, CancellationToken cancellationToken);
    void Remove(Order order);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}