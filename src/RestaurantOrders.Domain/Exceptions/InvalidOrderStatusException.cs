namespace RestaurantOrders.Domain.Exceptions;

/// <summary>
/// Exception raised when order status transition is invalid
/// </summary>
public class InvalidOrderStatusException : DomainException
{
    public InvalidOrderStatusException(string message) : base(message) { }
}
