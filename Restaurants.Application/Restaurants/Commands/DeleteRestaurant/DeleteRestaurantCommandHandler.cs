using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository) : IRequestHandler<DeleteRestaurantCommand, bool> {
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken) {
        logger.LogInformation($"Deleting restaurant with id : {request.Id}");

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);

        if (restaurant is null) {
            return false;
        }

        await restaurantsRepository.Delete(restaurant);
        return true;
    }
}
