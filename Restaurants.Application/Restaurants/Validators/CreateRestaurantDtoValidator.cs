using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Validators; 
public class CreateRestaurantDtoValidator : AbstractValidator<CreateRestaurantDto> {

    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian", "Greek"];
    public CreateRestaurantDtoValidator() {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        #region Category checks 
        RuleFor(dto => dto.Category)
            .NotEmpty()
            .WithMessage("Category is required.");

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .When(dto => !string.IsNullOrWhiteSpace(dto.Category))
            .WithMessage(dto =>
                $"Invalid category '{dto.Category}'. " +
                $"Please choose one of the following: {string.Join(", ", validCategories)}."
            );

        //RuleFor(dto => dto.Category)
        //.Custom((value, context) => {
        //    var isValidCategory = validCategories.Contains(value);
        //    if (!isValidCategory) {
        //        context.AddFailure("Category", "Invalid category. Please choose from the valid categories.");
        //    }
        //})
        #endregion

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
