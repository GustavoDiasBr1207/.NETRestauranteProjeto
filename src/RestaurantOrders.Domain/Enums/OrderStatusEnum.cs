namespace RestaurantOrders.Domain.Enums;

public enum OrderStatusEnum
{
    Draft = 0,
    Pending = 1,
    Confirmed = 2,
    Preparing = 3,
    Ready = 4,
    Delivered = 5,
    Cancelled = 6
}
