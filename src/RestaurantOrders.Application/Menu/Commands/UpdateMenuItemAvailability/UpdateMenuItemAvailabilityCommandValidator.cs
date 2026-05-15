namespace RestaurantOrders.Application.Menu.Commands.UpdateMenuItemAvailability;

using FluentValidation;

public class UpdateMenuItemAvailabilityCommandValidator : AbstractValidator<UpdateMenuItemAvailabilityCommand>
{
    public UpdateMenuItemAvailabilityCommandValidator()
    {
        RuleFor(x => x.MenuItemId)
            .NotEmpty()
            .WithMessage("O ID do item de cardápio é obrigatório.");
    }
}
