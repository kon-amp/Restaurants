using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.User;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.User.Commands.UnnasignUserRole;
public class UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger,
    IApplicationUserManager userManager,
    IApplicationRoleManager roleManager) : IRequestHandler<UnassignUserRoleCommand> {
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken) {
        logger.LogInformation("Unassigning user role {@Request}", request);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException(nameof(IApplicationUser), request.UserEmail);

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException(nameof(IApplicationRoleManager), request.RoleName);

        await userManager.RemoveFromRoleAsync(user.Id, role.Name!);
    }
}
