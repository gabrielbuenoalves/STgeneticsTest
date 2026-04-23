using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<List<OrderResponse>> ListAsync(CancellationToken cancellationToken);
    Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<OrderResponse> UpdateAsync(int id, UpdateOrderRequest request, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}