using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Abstractions.User;

namespace Restaurants.Infrastructure.Identity;
public class ApplicationRoleManager : IApplicationRoleManager {

    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationRoleManager(RoleManager<IdentityRole> roleManager) {
        _roleManager = roleManager;
    }

    public async Task<IdentityRole?> FindByNameAsync(string roleName) {
        return await _roleManager.FindByNameAsync(roleName);
    }

    public async Task<bool> RoleExistsAsync(string roleName) {
        return await _roleManager.RoleExistsAsync(roleName);
    }

}
