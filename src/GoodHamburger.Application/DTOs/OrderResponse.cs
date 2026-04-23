namespace GoodHamburger.Application.DTOs;

public sealed class OrderResponse
{
    public int Id { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public decimal Subtotal { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal Total { get; init; }
    public List<OrderItemResponse> Items { get; init; } = [];
}