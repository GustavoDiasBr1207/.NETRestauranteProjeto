namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using FluentValidation;

public class SubmitOrderCommandValidator : AbstractValidator<SubmitOrderCommand>
{
    public SubmitOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("O ID do pedido é obrigatório.");
    }
}
