using RestaurantOrders.Domain.Enums;

namespace RestaurantOrders.Domain.Exceptions;

public class InvalidOrderStatusException : DomainException
{
    public InvalidOrderStatusException(string message)
        : base(message) { }

    public InvalidOrderStatusException(OrderStatusEnum current, OrderStatusEnum attempted)
        : base($"Não é possível transicionar do status '{current}' para '{attempted}'.") { }
}