using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantsController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Gets all restaurants.
        /// </summary>
        /// <returns>All restaurants.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurants()
        {
            var restaurants = await _restaurantService.GetRestaurants();
            return Ok(restaurants);
        }

        /// <summary>
        /// Gets restaurant which Id is received as parameter.
        /// </summary>
        /// <param name="id">Restaurant Id.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<RestaurantResponse>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurant(id);

            if (restaurant.Value == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }

        /// <summary>
        /// Updates restaurant which Id is received as parameter.
        /// </summary>
        /// <param name="id">Restaurant Id.</param>
        /// <param name="restaurantRequest">Restaurant data.</param>
        /// <returns>Empty response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(long id, RestaurantRequest restaurantRequest)
        {
            if (id != restaurantRequest.Id)
            {
                return BadRequest();
            }

            await _restaurantService.Update(restaurantRequest);

            return NoContent();
        }

        /// <summary>
        /// Creates new restaurant.
        /// </summary>
        /// <param name="restaurantRequest">Restaurant data.</param>
        /// <returns>Created restaurant.</returns>
        [HttpPost]
        public async Task<ActionResult<RestaurantResponse>> PostRestaurant(RestaurantRequest restaurantRequest)
        {
            var restaurantResponse = await _restaurantService.Create(restaurantRequest);

            return CreatedAtAction("GetRestaurant", new { id = restaurantResponse.Value.Id }, restaurantResponse.Value);
        }

        /// <summary>
        /// Deletes restaurant which Id is received as parameter.
        /// </summary>
        /// <param name="id">Restaurant Id.</param>
        /// <returns>Deleted restaurant.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<RestaurantResponse>> DeleteRestaurant(int id)
        {
            var restaurant = await _restaurantService.DeleteRestaurant(id);
            if (restaurant.Value == null)
            {
                return NotFound();
            }

            return Ok(restaurant);
        }
    }
}
