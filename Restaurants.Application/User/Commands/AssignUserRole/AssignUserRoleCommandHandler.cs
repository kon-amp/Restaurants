using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.User;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.User.Commands.AssignUserRole;
public class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> logger,
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager) : IRequestHandler<AssignUserRoleCommand> {
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken) {
        logger.LogInformation("Assigning user role {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(IApplicationUser), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IApplicationRoleManager), request.RoleName);

        await userManager.AddToRoleAsync(user.Id, role.Name!);
    }
}
