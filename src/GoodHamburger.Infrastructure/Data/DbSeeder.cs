using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context, CancellationToken cancellationToken)
    {
        if (await context.MenuItems.AnyAsync(cancellationToken))
        {
            return;
        }

        var items = new[]
        {
            new MenuItem("X Burger", MenuItemType.Sandwich, 5.00m),
            new MenuItem("X Egg", MenuItemType.Sandwich, 4.50m),
            new MenuItem("X Bacon", MenuItemType.Sandwich, 7.00m),
            new MenuItem("Batata frita", MenuItemType.FrenchFries, 2.00m),
            new MenuItem("Refrigerante", MenuItemType.SoftDrink, 2.50m)
        };

        await context.MenuItems.AddRangeAsync(items, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}