namespace RestaurantOrders.Application.Menu.Commands.CreateMenuItem;

using MediatR;

/// <summary>Cria um novo item no cardápio do restaurante.</summary>
public record CreateMenuItemCommand(
    Guid    RestaurantId,
    Guid    CategoryId,
    string  Name,
    decimal Price,
    string? Description  = null,
    string? ImageUrl     = null,
    int     DisplayOrder = 0
) : IRequest<Guid>;
