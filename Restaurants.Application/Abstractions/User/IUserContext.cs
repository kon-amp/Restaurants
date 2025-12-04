using Restaurants.Application.User;

namespace Restaurants.Application.Abstractions.User {
    public interface IUserContext {
        CurrentUser? GetCurrentUser();
    }
}