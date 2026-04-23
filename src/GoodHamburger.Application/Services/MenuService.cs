using GoodHamburger.Application.Abstractions.Repositories;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;

namespace GoodHamburger.Application.Services;

public sealed class MenuService(IMenuItemRepository menuItemRepository) : IMenuService
{
    public async Task<List<MenuItemResponse>> ListAsync(CancellationToken cancellationToken)
    {
        var items = await menuItemRepository.ListAsync(cancellationToken);

        return items
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Name)
            .Select(x => new MenuItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type.ToString(),
                Price = x.Price
            })
            .ToList();
    }
}