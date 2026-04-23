using GoodHamburger.Application.DTOs;
using System.Text.Json;

namespace GoodHamburger.Web.Services;

public sealed class GoodHamburgerApiClient(HttpClient httpClient) : IGoodHamburgerApiClient
{
    public async Task<List<MenuItemResponse>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<MenuItemResponse>>("api/menu", cancellationToken) ?? [];
    }

    public async Task<List<OrderResponse>> GetOrdersAsync(CancellationToken cancellationToken = default)
    {
        return await httpClient.GetFromJsonAsync<List<OrderResponse>>("api/orders", cancellationToken) ?? [];
    }

    public async Task<ApiOperationResult> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/orders", request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return new ApiOperationResult(true);
        }

        return new ApiOperationResult(false, await ExtractErrorMessageAsync(response, cancellationToken));
    }

    private static async Task<string> ExtractErrorMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var payload = await response.Content.ReadAsStringAsync(cancellationToken);

        try
        {
            using var document = JsonDocument.Parse(payload);

            if (document.RootElement.TryGetProperty("detail", out var detail))
            {
                return detail.GetString() ?? payload;
            }

            if (document.RootElement.TryGetProperty("title", out var title))
            {
                return title.GetString() ?? payload;
            }
        }
        catch
        {
        }

        return payload;
    }
}
