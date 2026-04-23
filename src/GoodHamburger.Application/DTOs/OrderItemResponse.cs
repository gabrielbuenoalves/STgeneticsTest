namespace GoodHamburger.Application.DTOs;

public sealed class OrderItemResponse
{
    public int MenuItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
}