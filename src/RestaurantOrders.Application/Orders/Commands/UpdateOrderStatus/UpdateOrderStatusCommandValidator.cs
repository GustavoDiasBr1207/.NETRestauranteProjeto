namespace RestaurantOrders.Application.Orders.Commands.UpdateOrderStatus;

using FluentValidation;

/// <summary>
/// Validator for UpdateOrderStatusCommand
/// </summary>
public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required");
    }
}
