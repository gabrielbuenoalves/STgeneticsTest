using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;

namespace GoodHamburger.Tests.Unit;

public sealed class OrderRulesTests
{
    [Fact]
    public void CreateOrder_WithSandwichAndFriesAndSoftDrink_Applies20PercentDiscount()
    {
        var items = new List<MenuItem>
        {
            CreateMenuItem(1, "X Burger", MenuItemType.Sandwich, 5.00m),
            CreateMenuItem(2, "Batata frita", MenuItemType.FrenchFries, 2.00m),
            CreateMenuItem(3, "Refrigerante", MenuItemType.SoftDrink, 2.50m)
        };

        var order = new Order(items);

        order.Subtotal.Should().Be(9.50m);
        order.DiscountAmount.Should().Be(1.90m);
        order.Total.Should().Be(7.60m);
    }

    [Fact]
    public void CreateOrder_WithSandwichAndSoftDrink_Applies15PercentDiscount()
    {
        var items = new List<MenuItem>
        {
            CreateMenuItem(1, "X Egg", MenuItemType.Sandwich, 4.50m),
            CreateMenuItem(3, "Refrigerante", MenuItemType.SoftDrink, 2.50m)
        };

        var order = new Order(items);

        order.Subtotal.Should().Be(7.00m);
        order.DiscountAmount.Should().Be(1.05m);
        order.Total.Should().Be(5.95m);
    }

    [Fact]
    public void CreateOrder_WithSandwichAndFries_Applies10PercentDiscount()
    {
        var items = new List<MenuItem>
        {
            CreateMenuItem(1, "X Burger", MenuItemType.Sandwich, 5.00m),
            CreateMenuItem(2, "Batata frita", MenuItemType.FrenchFries, 2.00m)
        };

        var order = new Order(items);

        order.Subtotal.Should().Be(7.00m);
        order.DiscountAmount.Should().Be(0.70m);
        order.Total.Should().Be(6.30m);
    }

    [Fact]
    public void CreateOrder_WithDuplicatedType_ThrowsDomainException()
    {
        var items = new List<MenuItem>
        {
            CreateMenuItem(1, "X Burger", MenuItemType.Sandwich, 5.00m),
            CreateMenuItem(4, "X Bacon", MenuItemType.Sandwich, 7.00m)
        };

        var action = () => new Order(items);

        action.Should().Throw<DomainException>()
            .WithMessage("*duplicados*");
    }

    [Fact]
    public void CreateOrder_WithoutSandwich_ThrowsDomainException()
    {
        var items = new List<MenuItem>
        {
            CreateMenuItem(2, "Batata frita", MenuItemType.FrenchFries, 2.00m)
        };

        var action = () => new Order(items);

        action.Should().Throw<DomainException>()
            .WithMessage("*sanduiche*");
    }

    private static MenuItem CreateMenuItem(int id, string name, MenuItemType type, decimal price)
    {
        var menuItem = new MenuItem(name, type, price);
        typeof(MenuItem).GetProperty(nameof(MenuItem.Id))!.SetValue(menuItem, id);
        return menuItem;
    }
}