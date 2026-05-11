using System;

namespace RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Exception raised when trying to add an unavailable menu item
/// </summary>
public class MenuItemUnavailableException : DomainException
{
    public MenuItemUnavailableException(string message)
        : base(message) { }

    public MenuItemUnavailableException(Guid menuItemId)
        : base($"MenuItem '{menuItemId}' está indisponível no momento.") { }
}