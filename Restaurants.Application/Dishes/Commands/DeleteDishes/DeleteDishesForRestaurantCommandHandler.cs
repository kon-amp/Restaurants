
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Dishes.Commands.DeleteDishes;
public class DeleteDishesForRestaurantCommandHandler(ILogger<DeleteDishesForRestaurantCommand> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository)
    : IRequestHandler<DeleteDishesForRestaurantCommand> {
    public async Task Handle(DeleteDishesForRestaurantCommand request, CancellationToken cancellationToken) {
        logger.LogWarning("Removing all dishes from restaurant: {RestaurantId}", request.RestaurantId);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId)
            ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        await dishesRepository.Delete(restaurant.Dishes);
    }
}
