namespace RestaurantOrders.Application.Orders.Commands.SubmitOrder;

using FluentValidation;

/// <summary>
/// Validator for SubmitOrderCommand
/// </summary>
public class SubmitOrderCommandValidator : AbstractValidator<SubmitOrderCommand>
{
    public SubmitOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
    }
}
