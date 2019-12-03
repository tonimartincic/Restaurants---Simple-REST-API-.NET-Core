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
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        private readonly IRestaurantService _restaurantService;

        public CitiesController(ICityService cityService, IRestaurantService restaurantService)
        {
            _cityService = cityService;
            _restaurantService = restaurantService;
        }

        /// <summary>
        /// Gets all cities.
        /// </summary>
        /// <returns>All cities.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            return await _cityService.GetCities();
        }

        /// <summary>
        /// Gets city for given Id.
        /// </summary>
        /// <param name="id">City Id.</param>
        /// <returns>City for given Id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CityResponse>> GetCity(int id)
        {
            var city = await _cityService.GetCity(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        /// <summary>
        /// Updates city which Id is received as parameter.
        /// </summary>
        /// <param name="id">City Id.</param>
        /// <param name="cityRequest">City Data.</param>
        /// <returns>Empty response.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(long id, CityRequest cityRequest)
        {
            if (id != cityRequest.Id)
            {
                return BadRequest();
            }

            await _cityService.Update(cityRequest);

            return NoContent();
        }

        /// <summary>
        /// Creates new city.
        /// </summary>
        /// <param name="cityRequest">City data.</param>
        /// <returns>Created city.</returns>
        [HttpPost]
        public async Task<ActionResult<CityResponse>> PostCity(CityRequest cityRequest)
        {
            var cityResponse = await _cityService.Create(cityRequest);

            return CreatedAtAction("GetCity", new { id = cityResponse.Value.Id }, cityResponse.Value);
        }

        /// <summary>
        /// Deletes city which Id is received as parameter.
        /// </summary>
        /// <param name="id">City Id.</param>
        /// <returns>Deleted city.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CityResponse>> DeleteCity(int id)
        {
            var city = await _cityService.DeleteCity(id);
            if (city.Value == null)
            {
                return NotFound();
            }

            return city;
        }

        /// <summary>
        /// Gets restaurants for city which Id is received as parameter.
        /// </summary>
        /// <param name="id">City Id.</param>
        /// <returns>Restaurants for city which Id is received as parameter.</returns>
        [HttpGet("{id}/restaurants")]
        public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurantsForCity(int id)
        {
            return await _restaurantService.GetRestaurantsForCity(id);
        }
    }
}
