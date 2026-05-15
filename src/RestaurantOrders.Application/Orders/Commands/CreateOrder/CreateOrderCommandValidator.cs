namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using FluentValidation;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("O ID do restaurante é obrigatório.");

        RuleFor(x => x.TableId)
            .NotEmpty()
            .WithMessage("O ID da mesa é obrigatório.");
    }
}
