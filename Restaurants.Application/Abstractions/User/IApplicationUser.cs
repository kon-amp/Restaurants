namespace Restaurants.Application.Abstractions.User; 
public interface IApplicationUser {
    DateOnly? DateOfBirth { get; set; }
    string? Nationality { get; set; }
    string Id { get; }          // needed for user operations
    string? Email { get; }       // useful lookup

}
