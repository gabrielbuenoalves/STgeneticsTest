using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class OrderLine
{
    private OrderLine()
    {
    }

    internal OrderLine(MenuItem menuItem)
    {
        MenuItemId = menuItem.Id;
        MenuItemName = menuItem.Name;
        MenuItemType = menuItem.Type;
        UnitPrice = menuItem.Price;
    }

    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int MenuItemId { get; private set; }
    public string MenuItemName { get; private set; } = string.Empty;
    public MenuItemType MenuItemType { get; private set; }
    public decimal UnitPrice { get; private set; }
}