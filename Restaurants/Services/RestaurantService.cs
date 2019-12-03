using Microsoft.AspNetCore.Mvc;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Data.EFCore;
using Restaurants.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantRepository _restaurantRepository;

        private readonly ICityService _cityService;

        public RestaurantService(RestaurantRepository restaurantRepository, ICityService cityService)
        {
            _restaurantRepository = restaurantRepository;
            _cityService = cityService;
        }

        public async Task<ActionResult<RestaurantResponse>> Create(RestaurantRequest restaurantRequest)
        {
            var restaurant = MapRequestToModel(restaurantRequest);

            restaurant = await _restaurantRepository.Add(restaurant);

            var restaurantResponse = MapModelToResponse(restaurant);
            return restaurantResponse;
        }

        public async Task<ActionResult<RestaurantResponse>> DeleteRestaurant(int id)
        {
            var restaurant = await _restaurantRepository.Delete(id);

            var restaurantResponse = MapModelToResponse(restaurant);
            return restaurantResponse;
        }

        public async Task<ActionResult<RestaurantResponse>> GetRestaurant(int id)
        {
            var restaurant = await _restaurantRepository.Get(id);

            var restaurantResponse = MapModelToResponse(restaurant);
            return restaurantResponse;
        }

        public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAll();

            var restaurantResponses = MapModelToResponse(restaurants);
            return restaurantResponses;
        }

        public async Task<ActionResult<RestaurantResponse>> Update(RestaurantRequest restaurantRequest)
        {
            var restaurant = MapRequestToModel(restaurantRequest);

            restaurant = await _restaurantRepository.Update(restaurant);

            var restaurantResponse = MapModelToResponse(restaurant);
            return restaurantResponse;
        }

        public async Task<ActionResult<IEnumerable<RestaurantResponse>>> GetRestaurantsForCity(int id)
        {
            var restaurants =  _restaurantRepository.GetRestaurantsForCity(id);

            var restaurantResponses = MapModelToResponse(restaurants);
            return restaurantResponses;
        }

        public Restaurant MapRequestToModel(RestaurantRequest restaurantRequest)
        {
            if (restaurantRequest == null)
            {
                return null;
            }

            var restaurant = new Restaurant
            {
                Id = restaurantRequest.Id,
                Name = restaurantRequest.Name,
                CityId = restaurantRequest.CityId
            };

            return restaurant;
        }

        public List<Restaurant> MapRequestToModel(List<RestaurantRequest> restaurantRequests)
        {
            if (restaurantRequests == null)
            {
                return null;
            }

            var restaurants = new List<Restaurant>();
            foreach (var restaurantRequest in restaurantRequests)
            {
                var restaurant = MapRequestToModel(restaurantRequest);
                restaurants.Add(restaurant);
            }

            return restaurants;
        }

        public RestaurantResponse MapModelToResponse(Restaurant restaurant)
        {
            if (restaurant == null)
            {
                return null;
            }

            var restaurantResponse = new RestaurantResponse
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                CityResponse = _cityService.MapModelToResponse(restaurant.City)
            };

            return restaurantResponse;
        }

        public List<RestaurantResponse> MapModelToResponse(List<Restaurant> restaurants)
        {
            if (restaurants == null)
            {
                return null;
            }

            var restaurantResponses = new List<RestaurantResponse>();
            foreach (var restaurant in restaurants)
            {
                var restaurantResponse = MapModelToResponse(restaurant);
                restaurantResponses.Add(restaurantResponse);
            }

            return restaurantResponses;
        }
    }
}
