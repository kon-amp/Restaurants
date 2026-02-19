using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Abstractions.User;

namespace Restaurants.Infrastructure.Identity;
public class ApplicationUserManager : IApplicationUserManager {

    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserManager(UserManager<ApplicationUser> userManager) {
        _userManager = userManager;
    }

    public async Task<IApplicationUser?> FindByIdAsync(string userId) {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task<IApplicationUser?> FindByEmailAsync(string email) {
        return await _userManager.FindByEmailAsync(email);
    }


    public async Task<bool> AddToRoleAsync(string userId, string roleName) {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(string userId, string roleName) {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return false;

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }

}
