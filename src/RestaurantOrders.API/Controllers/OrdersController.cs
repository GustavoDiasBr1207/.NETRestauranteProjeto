namespace RestaurantOrders.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using MediatR;

/// <summary>
/// Orders API endpoints
/// POST   /orders                      → CreateOrder
/// POST   /orders/{id}/items           → AddItemToOrder
/// DELETE /orders/{id}/items/{itemId}  → RemoveItemFromOrder
/// POST   /orders/{id}/submit          → SubmitOrder
/// PATCH  /orders/{id}/status          → UpdateOrderStatus  [Authorize: Kitchen]
/// DELETE /orders/{id}                 → CancelOrder
/// GET    /orders/{id}                 → GetOrderById
/// GET    /orders?tableId=&status=     → GetOrdersByTable / GetActiveByRestaurant
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // TODO: Implement POST /orders
    // TODO: Implement GET /orders/{id}
    // TODO: Implement POST /orders/{id}/items
    // TODO: Implement DELETE /orders/{id}/items/{itemId}
    // TODO: Implement POST /orders/{id}/submit
    // TODO: Implement PATCH /orders/{id}/status
    // TODO: Implement DELETE /orders/{id}
    // TODO: Implement GET /orders with filtering
}
