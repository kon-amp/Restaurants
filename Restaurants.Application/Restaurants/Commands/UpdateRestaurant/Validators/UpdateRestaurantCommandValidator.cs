using FluentValidation;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Validators; 
public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand> {
    public UpdateRestaurantCommandValidator() {
        RuleFor(dto => dto.Name)
            .Length(3, 100);
    }
}
