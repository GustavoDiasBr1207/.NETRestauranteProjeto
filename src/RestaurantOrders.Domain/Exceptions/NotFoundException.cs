namespace RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Exception raised when an entity is not found
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}
