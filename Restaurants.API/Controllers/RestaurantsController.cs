using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Restaurants.API.Extensions;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

// The [ApiController] attribute automatically validates incoming requests 
// based on the type of the request model.
// It also assumes parameters are passed from the request body ([FromBody]) 
// if no other source is specified.
[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(
    IRestaurantsService restaurantsService, 
    IValidator<CreateRestaurantDto> createValidator) : ControllerBase {

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id) {
        var restaurant = await restaurantsService.GetRestaurantById(id);
        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody]CreateRestaurantDto createRestaurantDto) {
        #region Manual request validation (used if [ApiController] is not applied)
        // If the [ApiController] attribute isn’t used, we need to manually check 
        // whether the ModelState is valid before processing the request.
        //if (!ModelState.IsValid) {
        //    return BadRequest(ModelState);
        //}
        #endregion
        int id = await restaurantsService.Create(createRestaurantDto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
