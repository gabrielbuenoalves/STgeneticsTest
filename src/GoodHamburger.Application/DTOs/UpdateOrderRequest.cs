namespace GoodHamburger.Application.DTOs;

public sealed class UpdateOrderRequest
{
    public List<int> ItemIds { get; init; } = [];
}