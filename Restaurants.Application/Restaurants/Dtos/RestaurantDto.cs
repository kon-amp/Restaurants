using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;
public class RestaurantDto {
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }

    public List<DishDto> Dishes { get; set; } = [];

    #region Manual mapping example (if not using AutoMapper)
    // This static factory method shows how to map a Restaurant entity to RestaurantDto manually.
    // It is not used in the current project — kept for educational/reference purposes.
    //

    //public static RestaurantDto? FromEntity(Restaurant? restaurant) {
    //    if (restaurant is null) {
    //        return null;
    //    }

    //    return new() {
    //        Category = restaurant.Category,
    //        Description = restaurant.Description,
    //        Id = restaurant.Id,
    //        HasDelivery = restaurant.HasDelivery,
    //        Name = restaurant.Name,
    //        City = restaurant.Address?.City,
    //        Street = restaurant.Address?.Street,
    //        PostalCode = restaurant.Address?.PostalCode,
    //        Dishes = [.. restaurant.Dishes.Select(DishDto.FromEntity)]  // .ToList() logic
    //    };
    //}
    #endregion
}
