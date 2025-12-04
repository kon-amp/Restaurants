namespace Restaurants.Application.Abstractions.User; 
public interface IApplicationUser {
    DateOnly? DateOfBirth { get; set; }
    string? Nationality { get; set; }
}
