namespace RestaurantOrders.Domain.Exceptions;

/// <summary>Lançada ao tentar adicionar um item do cardápio marcado como indisponível.</summary>
public class MenuItemUnavailableException : DomainException
{
    public MenuItemUnavailableException(string message) : base(message) { }

    public MenuItemUnavailableException(Guid menuItemId)
        : base($"Item '{menuItemId}' está indisponível no momento.") { }
}
