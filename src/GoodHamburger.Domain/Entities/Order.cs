using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Services;

namespace GoodHamburger.Domain.Entities;

public class Order
{
    private readonly List<OrderLine> _lines = [];

    private Order()
    {
    }

    public Order(IEnumerable<MenuItem> selectedItems)
    {
        ReplaceItems(selectedItems);
    }

    public int Id { get; private set; }
    public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
    public decimal Subtotal { get; private set; }
    public decimal DiscountAmount { get; private set; }
    public decimal Total { get; private set; }
    public IReadOnlyCollection<OrderLine> Lines => _lines.AsReadOnly();

    public void ReplaceItems(IEnumerable<MenuItem> selectedItems)
    {
        var menuItems = selectedItems?.ToList() ?? throw new DomainException("Pedido invalido: nenhum item informado.");

        if (menuItems.Count == 0)
        {
            throw new DomainException("Pedido invalido: informe ao menos um item.");
        }

        if (menuItems.All(x => x.Type != MenuItemType.Sandwich))
        {
            throw new DomainException("Pedido invalido: o pedido deve conter um sanduiche.");
        }

        ValidateNoDuplicateTypes(menuItems);

        _lines.Clear();
        _lines.AddRange(menuItems.Select(item => new OrderLine(item)));

        RecalculateTotals();
    }

    private void ValidateNoDuplicateTypes(List<MenuItem> menuItems)
    {
        var duplicatedType = menuItems
            .GroupBy(x => x.Type)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicatedType is null)
        {
            return;
        }

        var typeName = duplicatedType.Key switch
        {
            MenuItemType.Sandwich => "sanduiche",
            MenuItemType.FrenchFries => "batata",
            MenuItemType.SoftDrink => "refrigerante",
            _ => "item"
        };

        throw new DomainException($"Itens duplicados nao permitidos: apenas um {typeName} por pedido.");
    }

    private void RecalculateTotals()
    {
        Subtotal = decimal.Round(_lines.Sum(x => x.UnitPrice), 2, MidpointRounding.AwayFromZero);

        var hasSandwich = _lines.Any(x => x.MenuItemType == MenuItemType.Sandwich);
        var hasFrenchFries = _lines.Any(x => x.MenuItemType == MenuItemType.FrenchFries);
        var hasSoftDrink = _lines.Any(x => x.MenuItemType == MenuItemType.SoftDrink);

        var discountRate = DiscountPolicy.CalculateRate(hasSandwich, hasFrenchFries, hasSoftDrink);
        DiscountAmount = decimal.Round(Subtotal * discountRate, 2, MidpointRounding.AwayFromZero);
        Total = decimal.Round(Subtotal - DiscountAmount, 2, MidpointRounding.AwayFromZero);
    }
}