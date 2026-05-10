namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using MediatR;
using RestaurantOrders.Domain.Interfaces.Repositories;
using RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Command to update menu item availability
/// </summary>
public class UpdateMenuItemAvailabilityCommand : IRequest<Unit>
{
    public Guid MenuItemId { get; set; }
    public bool IsAvailable { get; set; }
}

/// <summary>
/// Handler for UpdateMenuItemAvailabilityCommand
/// </summary>
public class UpdateMenuItemAvailabilityCommandHandler : IRequestHandler<UpdateMenuItemAvailabilityCommand>
{
    private readonly IMenuRepository _menuRepository;
    
    public UpdateMenuItemAvailabilityCommandHandler(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }
    
    public async Task Handle(UpdateMenuItemAvailabilityCommand request, CancellationToken cancellationToken)
    {
        // TODO: Implement logic to update menu item availability
        var menuItem = await _menuRepository.GetItemByIdAsync(request.MenuItemId, cancellationToken)
            ?? throw new NotFoundException($"Menu item with id '{request.MenuItemId}' not found");
        
        menuItem.IsAvailable = request.IsAvailable;
        
        await _menuRepository.UpdateMenuItemAsync(menuItem, cancellationToken);
    }
}
