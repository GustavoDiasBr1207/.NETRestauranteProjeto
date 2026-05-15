namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

public record UpdateMenuItemAvailabilityCommand(Guid MenuItemId, bool IsAvailable) : IRequest;

public class UpdateMenuItemAvailabilityCommandHandler(IMenuRepository menuRepository)
    : IRequestHandler<UpdateMenuItemAvailabilityCommand>
{
    public async Task Handle(UpdateMenuItemAvailabilityCommand request, CancellationToken ct)
    {
        var menuItem = await menuRepository.GetItemByIdAsync(request.MenuItemId, ct)
            ?? throw new NotFoundException($"Item de menu '{request.MenuItemId}' não encontrado.");

        if (request.IsAvailable)
            menuItem.MakeAvailable();
        else
            menuItem.MakeUnavailable();

        await menuRepository.UpdateMenuItemAsync(menuItem, ct);
    }
}
