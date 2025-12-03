using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Abstractions.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository
    ) : IRequestHandler<UpdateRestaurantCommand> {
    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken) {
        logger.LogInformation("Updating restaurant with id : {RestaurantId} with {@UpdateRestaurant}", request.Id, request);

        var restaurant = await restaurantsRepository.GetByIdAsync(request.Id);
        if (restaurant is null) {
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        mapper.Map(request, restaurant);

        await restaurantsRepository.SaveChanges();
    }
}
