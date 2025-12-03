using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
public class CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IMapper mapper) : IRequestHandler<CreateDishCommand, int> {
    public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken) {
        logger.LogInformation("Creating new dish: {@DishRequest}", request);
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId) 
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = mapper.Map<Dish>(request);

        return await dishesRepository.Create(dish);
    }
}
