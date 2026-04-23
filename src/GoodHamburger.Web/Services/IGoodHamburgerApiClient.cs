using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Web.Services;

public interface IGoodHamburgerApiClient
{
    Task<List<MenuItemResponse>> GetMenuAsync(CancellationToken cancellationToken = default);
    Task<List<OrderResponse>> GetOrdersAsync(CancellationToken cancellationToken = default);
    Task<ApiOperationResult> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<ApiOperationResult> UpdateOrderAsync(int orderId, UpdateOrderRequest request, CancellationToken cancellationToken = default);
    Task<ApiOperationResult> DeleteOrderAsync(int orderId, CancellationToken cancellationToken = default);
}
