using FluentValidation;

namespace Restaurants.Application.User.Commands.UnnasignUserRole.Validators; 
public class UnassignUserRoleCommandValidator : AbstractValidator<UnassignUserRoleCommand> {
    public UnassignUserRoleCommandValidator() {
        RuleFor(user => user.UserEmail)
            .NotEmpty()
            .WithMessage("User email is required.");

        RuleFor(user => user.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required.");
    }
}
