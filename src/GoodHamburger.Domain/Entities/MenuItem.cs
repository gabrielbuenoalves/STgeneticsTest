using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class MenuItem
{
    private MenuItem()
    {
    }

    public MenuItem(string name, MenuItemType type, decimal price)
    {
        Name = string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("Nome do item do cardapio e obrigatorio.", nameof(name))
            : name.Trim();
        Type = type;
        Price = price > 0 ? price : throw new ArgumentOutOfRangeException(nameof(price), "Preco deve ser maior que zero.");
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public MenuItemType Type { get; private set; }
    public decimal Price { get; private set; }
}