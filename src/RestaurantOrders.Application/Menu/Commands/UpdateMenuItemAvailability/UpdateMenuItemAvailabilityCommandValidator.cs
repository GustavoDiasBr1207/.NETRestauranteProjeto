namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using FluentValidation;

/// <summary>
/// Validator for UpdateMenuItemAvailabilityCommand
/// </summary>
public class UpdateMenuItemAvailabilityCommandValidator : AbstractValidator<UpdateMenuItemAvailabilityCommand>
{
    public UpdateMenuItemAvailabilityCommandValidator()
    {
        RuleFor(x => x.MenuItemId)
            .NotEmpty()
            .WithMessage("MenuItemId is required");
    }
}
