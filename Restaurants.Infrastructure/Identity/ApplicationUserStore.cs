using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Abstractions.User;

namespace Restaurants.Infrastructure.Identity; 
public class ApplicationUserStore(UserManager<ApplicationUser> userManager) : IApplicationUserStore {
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<IApplicationUser?> FindByIdAsync(string id, CancellationToken cancellationToken = default) {
        // UserManager handles cancellation internally, so we just pass id
        return await _userManager.FindByIdAsync(id);
    }

    public async Task UpdateAsync(IApplicationUser user, CancellationToken cancellationToken = default) {
        // We know at runtime this will be an ApplicationUser
        var concrete = (ApplicationUser)user;
        await _userManager.UpdateAsync(concrete);
    }
}
