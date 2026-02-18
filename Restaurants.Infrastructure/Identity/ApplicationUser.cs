using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Abstractions.User;

namespace Restaurants.Infrastructure.Identity;
public class ApplicationUser : IdentityUser, IApplicationUser {
    // Id and Email already come from IdentityUser
    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }
}
