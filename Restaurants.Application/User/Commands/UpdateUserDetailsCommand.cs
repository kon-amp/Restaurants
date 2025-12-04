using MediatR;

namespace Restaurants.Application.User.Commands; 
public class UpdateUserDetailsCommand : IRequest {
    public DateOnly? DateOfBirth { get; set; }

    public string? Nationality { get; set; }
}
