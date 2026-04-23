namespace GoodHamburger.Application.DTOs;

public sealed class CreateOrderRequest
{
    public List<int> ItemIds { get; init; } = [];
}