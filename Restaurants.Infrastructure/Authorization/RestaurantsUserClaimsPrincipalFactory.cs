using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Infrastructure.Identity;
using System.Security.Claims;

namespace Restaurants.Infrastructure.Authorization;
public class RestaurantsUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, 
    RoleManager<IdentityRole> roleManager, 
    IOptions<IdentityOptions> options) 
        : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, options) {

    public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user) {
        var id = await GenerateClaimsAsync(user);

        if (user.Nationality is not null) {
            id.AddClaim(new Claim("Nationality", user.Nationality));
        }

        if (user.DateOfBirth is not null) {
            id.AddClaim(new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
        }

        return new ClaimsPrincipal(id);
    }
}
