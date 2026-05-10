namespace RestaurantOrders.Application.Orders.Commands.CreateOrder;

using FluentValidation;

/// <summary>
/// Validator for CreateOrderCommand
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.RestaurantId)
            .NotEmpty()
            .WithMessage("RestaurantId is required");
        
        RuleFor(x => x.TableId)
            .NotEmpty()
            .WithMessage("TableId is required");
    }
}
