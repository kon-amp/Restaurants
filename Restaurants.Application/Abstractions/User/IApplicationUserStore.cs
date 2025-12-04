namespace Restaurants.Application.Abstractions.User; 
public interface IApplicationUserStore {
    Task<IApplicationUser?> FindByIdAsync(string id, CancellationToken cancellationToken = default);
    Task UpdateAsync(IApplicationUser user, CancellationToken cancellationToken = default);
}
