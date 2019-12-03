using Microsoft.AspNetCore.Mvc;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public interface IRestaurantService
    {
        Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurants();

        Task<ActionResult<RestaurantResponse>> GetRestaurant(int id);

        Task<ActionResult<RestaurantResponse>> Update(RestaurantRequest restaurantRequest);

        Task<ActionResult<RestaurantResponse>> Create(RestaurantRequest restaurantRequest);

        Task<ActionResult<RestaurantResponse>> DeleteRestaurant(int id);

        Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurantsForCity(int id);

        Restaurant MapRequestToModel(RestaurantRequest restaurantRequest);

        List<Restaurant> MapRequestToModel(List<RestaurantRequest> restaurantRequests);

        RestaurantResponse MapModelToResponse(Restaurant restaurant);

        List<RestaurantResponse> MapModelToResponse(List<Restaurant> restaurants);
    }
}
