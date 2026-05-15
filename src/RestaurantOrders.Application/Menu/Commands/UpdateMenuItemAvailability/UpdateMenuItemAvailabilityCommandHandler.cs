namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using MediatR;
using RestaurantOrders.Domain.Exceptions;
using RestaurantOrders.Domain.Interfaces.Repositories;

public class UpdateMenuItemAvailabilityCommandHandler(IMenuRepository menuRepository)
    : IRequestHandler<UpdateMenuItemAvailabilityCommand>
{
    public async Task Handle(UpdateMenuItemAvailabilityCommand request, CancellationToken ct)
    {
        var menuItem = await menuRepository.GetItemByIdAsync(request.MenuItemId, ct)
            ?? throw new NotFoundException($"Item de cardápio '{request.MenuItemId}' não encontrado.");

        if (request.IsAvailable)
            menuItem.MakeAvailable();
        else
            menuItem.MakeUnavailable();

        await menuRepository.UpdateMenuItemAsync(menuItem, ct);
    }
}
