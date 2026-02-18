using Microsoft.AspNetCore.Identity;

namespace Restaurants.Application.Abstractions.User;
public interface IApplicationRoleManager {
    Task<IdentityRole?> FindByNameAsync(string roleName);
    Task<bool> RoleExistsAsync(string roleName);

}
