namespace RestaurantOrders.Application.Orders.Commands.AddItemToOrder;

using FluentValidation;

/// <summary>
/// Validator for AddItemToOrderCommand
/// </summary>
public class AddItemToOrderCommandValidator : AbstractValidator<AddItemToOrderCommand>
{
    public AddItemToOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
        
        RuleFor(x => x.MenuItemId)
            .NotEmpty()
            .WithMessage("MenuItemId is required");
        
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
}
