using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant; 
public class GetDishByIdForRestaurantQueryHandler(ILogger<GetDishByIdForRestaurantQueryHandler> logger,
IRestaurantsRepository restaurantsRepository,
IMapper mapper) : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto> {
    public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken) {
        logger.LogInformation("Retrieving dish: {DishId} for restaurant with ID {RestaurantId}", request.DishId, request.RestaurantId);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId) 
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId) 
            ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        var result = mapper.Map<DishDto>(dish);

        return result;
    }
}
