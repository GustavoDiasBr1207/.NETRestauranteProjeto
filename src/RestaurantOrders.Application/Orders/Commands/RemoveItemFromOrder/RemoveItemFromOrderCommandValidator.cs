namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using FluentValidation;

public class RemoveItemFromOrderCommandValidator : AbstractValidator<RemoveItemFromOrderCommand>
{
    public RemoveItemFromOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório.");

        RuleFor(x => x.OrderItemId)
            .NotEmpty()
            .WithMessage("O ID do item do pedido é obrigatório.");
    }
}
