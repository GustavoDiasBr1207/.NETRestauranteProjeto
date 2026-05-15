namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using FluentValidation;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório.");
    }
}
