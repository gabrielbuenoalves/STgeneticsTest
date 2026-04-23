using GoodHamburger.Application.DTOs;
using GoodHamburger.Web.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Web.Pages;

public partial class BlazorOrders : ComponentBase
{
    private sealed class CreateOrderForm
    {
        [MinLength(1, ErrorMessage = "Selecione ao menos um item para criar o pedido.")]
        public List<int> ItemIds { get; set; } = [];
    }

    [Inject]
    private IGoodHamburgerApiClient ApiClient { get; set; } = default!;

    private CreateOrderForm _form = new();
    private List<MenuItemResponse>? _menuItems;
    private List<OrderResponse>? _orders;
    private string? _error;
    private readonly Dictionary<string, int> _selectedItemsByType = new();
    private bool _canSubmitOrder;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _menuItems = await ApiClient.GetMenuAsync();
        }
        catch (Exception ex)
        {
            _error = $"Erro ao carregar cardapio: {ex.Message}";
            _menuItems = [];
        }

        await LoadOrdersAsync();
    }

    private void ToggleItemSelection(string itemType, int itemId, bool isChecked)
    {
        if (isChecked)
        {
            _selectedItemsByType[itemType] = itemId;
        }
        else if (_selectedItemsByType.TryGetValue(itemType, out var selectedId) && selectedId == itemId)
        {
            _selectedItemsByType.Remove(itemType);
        }

        _form.ItemIds = _selectedItemsByType.Values.ToList();
        _canSubmitOrder = HasSelectedSandwich();
    }

    private bool IsSelected(string itemType, int itemId)
    {
        return _selectedItemsByType.TryGetValue(itemType, out var selectedId) && selectedId == itemId;
    }

    private static string GetGroupTitle(string itemType)
    {
        return itemType switch
        {
            "Sandwich" => "Sanduiche",
            "FrenchFries" => "Batata",
            "SoftDrink" => "Refrigerante",
            _ => itemType
        };
    }

    private async Task CreateOrderAsync()
    {
        _error = null;

        if (!HasSelectedSandwich())
        {
            _error = "Selecione um sanduiche antes de criar o pedido.";
            return;
        }

        var result = await ApiClient.CreateOrderAsync(new CreateOrderRequest
        {
            ItemIds = _form.ItemIds
        });

        if (!result.IsSuccess)
        {
            _error = result.ErrorMessage;
            return;
        }

        _form = new CreateOrderForm();
        _selectedItemsByType.Clear();
        _canSubmitOrder = false;
        await LoadOrdersAsync();
    }

    private bool HasSelectedSandwich()
    {
        if (_menuItems is null || _menuItems.Count == 0)
        {
            return false;
        }

        foreach (var selectedItemId in _selectedItemsByType.Values)
        {
            var selectedItem = _menuItems.FirstOrDefault(item => item.Id == selectedItemId);

            if (selectedItem?.Type == "Sandwich")
            {
                return true;
            }
        }

        return false;
    }

    private async Task LoadOrdersAsync()
    {
        try
        {
            _orders = await ApiClient.GetOrdersAsync() ?? [];
        }
        catch (Exception ex)
        {
            _error = $"Erro ao carregar pedidos: {ex.Message}";
            _orders = [];
        }
    }
}
