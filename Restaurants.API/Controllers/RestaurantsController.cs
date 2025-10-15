using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

// The [ApiController] attribute automatically validates incoming requests 
// based on the type of the request model.
// It also assumes parameters are passed from the request body ([FromBody]) 
// if no other source is specified.
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(
    IMediator mediator) : ControllerBase {

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        //var restaurants = await restaurantsService.GetAllRestaurants();
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id) {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));

        if (restaurant is null) {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id) {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));

        if (isDeleted) {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand command) {
        command.Id = id;
        var isUpdated = await mediator.Send(command);

        if (isUpdated) {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command) {
        #region Manual request validation (used if [ApiController] is not applied)
        // If the [ApiController] attribute isn’t used, we need to manually check 
        // whether the ModelState is valid before processing the request.
        //if (!ModelState.IsValid) {
        //    return BadRequest(ModelState);
        //}
        #endregion
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
