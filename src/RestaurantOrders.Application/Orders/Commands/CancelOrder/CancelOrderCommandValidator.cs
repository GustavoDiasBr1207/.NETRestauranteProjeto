namespace RestaurantOrders.Application.Orders.Commands.CancelOrder;

using FluentValidation;

/// <summary>
/// Validator for CancelOrderCommand
/// </summary>
public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
    }
}
