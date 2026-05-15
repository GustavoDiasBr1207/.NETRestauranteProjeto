namespace RestaurantOrders.Application.Menu.Commands.CreateMenuItem;

using MediatR;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.ValueObjects;
using RestaurantOrders.Domain.Interfaces.Repositories;

public record CreateMenuItemCommand(
    Guid    RestaurantId,
    Guid    CategoryId,
    string  Name,
    decimal Price,
    string? Description  = null,
    string? ImageUrl     = null,
    int     DisplayOrder = 0
) : IRequest<Guid>;

public class CreateMenuItemCommandHandler(IMenuRepository menuRepository)
    : IRequestHandler<CreateMenuItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateMenuItemCommand request, CancellationToken ct)
    {
        var price    = new Money(request.Price);
        var menuItem = MenuItem.Create(
            request.RestaurantId,
            request.CategoryId,
            request.Name,
            price,
            request.Description,
            request.ImageUrl,
            request.DisplayOrder);

        await menuRepository.AddMenuItemAsync(menuItem, ct);
        return menuItem.Id;
    }
}
