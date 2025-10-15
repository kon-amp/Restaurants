using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators; 
public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto> {
    public CreateRestaurantDtoValidator() {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(dto => dto.Category)
            .NotEmpty()
            .WithMessage("Insert a valid category");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress()
            .WithMessage("Please provide valid email address");

        RuleFor(dto => dto.ContactNumber)
            .Matches(@"^(\+?\d+)?$")
            .WithMessage("Please provide a valid phone number");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\\d{2}-\d{3}$")
            .WithMessage("Please provide a valid postal code (XX-XXX).");
    }
}
