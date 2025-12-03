using FluentValidation;

namespace Restaurants.Application.Dishes.Commands.CreateDish.Validators;
public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand> {
    public CreateDishCommandValidator() {
        RuleFor(dish => dish.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Price must be a non-negative number.");

        RuleFor(dish => dish.KiloCaleries)
            .GreaterThanOrEqualTo(0)
            .WithMessage("KiloCaleries must be a non-negative number.");
    }
}
