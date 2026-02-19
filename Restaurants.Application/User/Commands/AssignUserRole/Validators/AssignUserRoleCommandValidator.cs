using FluentValidation;

namespace Restaurants.Application.User.Commands.AssignUserRole.Validators; 
public class AssignUserRoleCommandValidator : AbstractValidator<AssignUserRoleCommand> {
    public AssignUserRoleCommandValidator() {
        RuleFor(user => user.UserEmail)
            .NotEmpty()
            .WithMessage("User email is required.");

        RuleFor(user => user.RoleName)
            .NotEmpty()
            .WithMessage("Role name is required.");
    }
}
