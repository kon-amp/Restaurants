using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Abstractions.User;

namespace Restaurants.Infrastructure.Identity;
public class ApplicationUser : IdentityUser, IApplicationUser {
    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }

}
