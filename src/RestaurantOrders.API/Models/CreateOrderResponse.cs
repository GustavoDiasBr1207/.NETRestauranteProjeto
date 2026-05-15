namespace RestaurantOrders.API.Models;

/// <summary>
/// Resposta ao criar um novo pedido.
/// </summary>
/// <param name="OrderId">ID do pedido recém-criado.</param>
public sealed record CreateOrderResponse(Guid OrderId);
