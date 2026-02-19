using MediatR;

namespace Restaurants.Application.User.Commands.UnnasignUserRole; 
public class UnassignUserRoleCommand : IRequest {
    public string UserEmail { get; set; } = default!;
    public string RoleName { get; set; } = default!;
}
