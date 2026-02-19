namespace Restaurants.Application.Abstractions.User;
public interface IApplicationUserManager {
    Task<IApplicationUser?> FindByIdAsync(string userId);
    Task<bool> AddToRoleAsync(string userId, string roleName);
    Task<IApplicationUser?> FindByEmailAsync(string email);
    Task<bool> RemoveFromRoleAsync(string userId, string roleName);
}

