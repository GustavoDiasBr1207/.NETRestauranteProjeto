namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using MediatR;

/// <summary>Habilita ou desabilita a disponibilidade de um item do cardápio.</summary>
public record UpdateMenuItemAvailabilityCommand(Guid MenuItemId, bool IsAvailable) : IRequest;
