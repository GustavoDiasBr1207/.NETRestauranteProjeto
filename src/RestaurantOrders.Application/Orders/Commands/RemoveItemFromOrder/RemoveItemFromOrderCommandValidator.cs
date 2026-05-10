namespace RestaurantOrders.Application.Orders.Commands.RemoveItemFromOrder;

using FluentValidation;

/// <summary>
/// Validator for RemoveItemFromOrderCommand
/// </summary>
public class RemoveItemFromOrderCommandValidator : AbstractValidator<RemoveItemFromOrderCommand>
{
    public RemoveItemFromOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
        
        RuleFor(x => x.OrderItemId)
            .NotEmpty()
            .WithMessage("OrderItemId is required");
    }
}
