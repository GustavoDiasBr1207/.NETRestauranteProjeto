namespace RestaurantOrders.Domain.Exceptions;

/// <summary>Lançada quando uma entidade buscada não existe no repositório.</summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string message) : base(message) { }
}
