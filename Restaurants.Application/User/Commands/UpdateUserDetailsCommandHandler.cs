using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.User;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.User.Commands;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserContext userContext,
    IApplicationUserStore userStore) 
    : IRequestHandler<UpdateUserDetailsCommand> {
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken) {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Updating user: {UserId}, with {@Request}", user!.Id ,request);

        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken)
            ?? throw new NotFoundException(nameof(IApplicationUser), user!.Id);

        dbUser.Nationality = request.Nationality;
        dbUser.DateOfBirth = request.DateOfBirth;

        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}
