namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using FluentValidation;

public class AddItemToOrderCommandValidator : AbstractValidator<AddItemToOrderCommand>
{
    public AddItemToOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório.");

        RuleFor(x => x.MenuItemId)
            .NotEmpty()
            .WithMessage("O ID do item de cardápio é obrigatório.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
