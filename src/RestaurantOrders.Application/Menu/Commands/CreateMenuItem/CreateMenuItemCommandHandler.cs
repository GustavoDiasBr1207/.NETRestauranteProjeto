namespace RestaurantOrders.Application.Menu.Commands.CreateMenuItem;

using MediatR;
using RestaurantOrders.Domain.Entities;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.ValueObjects;

public class CreateMenuItemCommandHandler(IMenuRepository menuRepository)
    : IRequestHandler<CreateMenuItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateMenuItemCommand request, CancellationToken ct)
    {
        var preco    = new Money(request.Price);
        var menuItem = MenuItem.Create(
            request.RestaurantId,
            request.CategoryId,
            request.Name,
            preco,
            request.Description,
            request.ImageUrl,
            request.DisplayOrder);

        await menuRepository.AddMenuItemAsync(menuItem, ct);
        return menuItem.Id;
    }
}
