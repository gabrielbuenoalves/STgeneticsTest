using GoodHamburger.Application.Abstractions.Repositories;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Exceptions;

namespace GoodHamburger.Application.Services;

public sealed class OrderService(IOrderRepository orderRepository, IMenuItemRepository menuItemRepository) : IOrderService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var selectedItems = await ResolveSelectedItemsAsync(request.ItemIds, cancellationToken);

        var order = new Order(selectedItems);
        await orderRepository.AddAsync(order, cancellationToken);
        await orderRepository.SaveChangesAsync(cancellationToken);

        return Map(order);
    }

    public async Task<List<OrderResponse>> ListAsync(CancellationToken cancellationToken)
    {
        var orders = await orderRepository.ListAsync(cancellationToken);
        return orders.Select(Map).ToList();
    }

    public async Task<OrderResponse> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Pedido nao encontrado.");

        return Map(order);
    }

    public async Task<OrderResponse> UpdateAsync(int id, UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Pedido nao encontrado.");

        var selectedItems = await ResolveSelectedItemsAsync(request.ItemIds, cancellationToken);
        order.ReplaceItems(selectedItems);
        await orderRepository.SaveChangesAsync(cancellationToken);

        return Map(order);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new ResourceNotFoundException("Pedido nao encontrado.");

        orderRepository.Remove(order);
        await orderRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<List<MenuItem>> ResolveSelectedItemsAsync(List<int> requestItemIds, CancellationToken cancellationToken)
    {
        if (requestItemIds is null || requestItemIds.Count == 0)
        {
            throw new DomainException("Pedido invalido: informe ao menos um item.");
        }

        var uniqueIds = requestItemIds.Distinct().ToList();
        var menuItems = await menuItemRepository.GetByIdsAsync(uniqueIds, cancellationToken);

        if (menuItems.Count != uniqueIds.Count)
        {
            throw new DomainException("Pedido invalido: um ou mais itens do cardapio nao existem.");
        }

        var menuById = menuItems.ToDictionary(x => x.Id);
        return requestItemIds.Select(id => menuById[id]).ToList();
    }

    private static OrderResponse Map(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            CreatedAtUtc = order.CreatedAtUtc,
            Subtotal = order.Subtotal,
            DiscountAmount = order.DiscountAmount,
            Total = order.Total,
            Items = order.Lines
                .Select(line => new OrderItemResponse
                {
                    MenuItemId = line.MenuItemId,
                    Name = line.MenuItemName,
                    Type = line.MenuItemType.ToString(),
                    UnitPrice = line.UnitPrice
                })
                .ToList()
        };
    }
}