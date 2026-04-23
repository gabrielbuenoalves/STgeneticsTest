using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IMenuService
{
    Task<List<MenuItemResponse>> ListAsync(CancellationToken cancellationToken);
}