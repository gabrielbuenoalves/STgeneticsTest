using GoodHamburger.Application.DTOs;
using GoodHamburger.Web.Services;
using Microsoft.AspNetCore.Components;

namespace GoodHamburger.Web.Pages;

public partial class BlazorMenu : ComponentBase
{
    [Inject]
    private IGoodHamburgerApiClient ApiClient { get; set; } = default!;

    private List<MenuItemResponse>? _items;

    protected override async Task OnInitializedAsync()
    {
        _items = await ApiClient.GetMenuAsync();
    }
}
