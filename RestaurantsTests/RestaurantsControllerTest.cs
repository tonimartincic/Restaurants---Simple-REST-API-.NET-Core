using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurants.Controllers;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Data;
using Restaurants.Data.EFCore;
using Restaurants.Models;
using Restaurants.Services;
using System.Collections.Generic;
using Xunit;

namespace RestaurantsTests
{
    public class RestaurantsControllerTest
    {
        public ApplicationDbContext ApplicationDbContext { get; set; }

        private RestaurantsController _restaurantsController;

        private IRestaurantService _restaurantService;

        private ICityService _cityService;

        private RestaurantRepository _restaurantRepository;

        private CityRepository _cityRepository;

        public RestaurantsControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Restaurants-testdb")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            ApplicationDbContext = new ApplicationDbContext(options);

            _cityRepository = new CityRepository(ApplicationDbContext);
            _restaurantRepository = new RestaurantRepository(ApplicationDbContext);

            _cityService = new CityService(_cityRepository);
            _restaurantService = new RestaurantService(_restaurantRepository, _cityService);

            _restaurantsController = new RestaurantsController(_restaurantService);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var city1 = new City
            {
                Id = 1,
                Name = "City 1"
            };
            var restaurant1 = new Restaurant
            {
                Id = 1,
                CityId = 1,
                Name = "Restaurant 1"
            };
            var city2 = new City
            {
                Id = 2,
                Name = "City 2"
            };
            var restaurant2 = new Restaurant
            {
                Id = 2,
                CityId = 2,
                Name = "Restaurant 2"
            };
            var city3 = new City
            {
                Id = 3,
                Name = "City 3"
            };
            var restaurant3 = new Restaurant
            {
                Id = 3,
                CityId = 3,
                Name = "Restaurant 3"
            };
            ApplicationDbContext.Add(city1);
            ApplicationDbContext.Add(restaurant1);
            ApplicationDbContext.Add(city2);
            ApplicationDbContext.Add(restaurant2);
            ApplicationDbContext.Add(city3);
            ApplicationDbContext.Add(restaurant3);
            ApplicationDbContext.SaveChanges();

            var result = _restaurantsController.GetRestaurants();

            Assert.IsType<OkObjectResult>(result.Result.Result);

            ApplicationDbContext.Remove(restaurant1);
            ApplicationDbContext.Remove(city1);
            ApplicationDbContext.Remove(restaurant2);
            ApplicationDbContext.Remove(city2);
            ApplicationDbContext.Remove(restaurant3);
            ApplicationDbContext.Remove(city3);
            ApplicationDbContext.SaveChanges();
        }
        
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            var city1 = new City
            {
                Id = 1,
                Name = "City 1"
            };
            var restaurant1 = new Restaurant
            {
                Id = 1,
                CityId = 1,
                Name = "Restaurant 1"
            };
            var city2 = new City
            {
                Id = 2,
                Name = "City 2"
            };
            var restaurant2 = new Restaurant
            {
                Id = 2,
                CityId = 2,
                Name = "Restaurant 2"
            };
            var city3 = new City
            {
                Id = 3,
                Name = "City 3"
            };
            var restaurant3 = new Restaurant
            {
                Id = 3,
                CityId = 3,
                Name = "Restaurant 3"
            };
            ApplicationDbContext.Add(city1);
            ApplicationDbContext.Add(restaurant1);
            ApplicationDbContext.Add(city2);
            ApplicationDbContext.Add(restaurant2);
            ApplicationDbContext.Add(city3);
            ApplicationDbContext.Add(restaurant3);
            ApplicationDbContext.SaveChanges();

            var okResult = _restaurantsController.GetRestaurants().Result.Result as OkObjectResult;

            var items = Assert.IsType<ActionResult<IEnumerable<RestaurantResponse>>>(okResult.Value);

            var restaurantResponses = items.Value;

            int numberOfRestaurants = 0;
            using (IEnumerator<RestaurantResponse> enumerator = restaurantResponses.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    numberOfRestaurants++;
                }
            }

            Assert.Equal(3, numberOfRestaurants);

            ApplicationDbContext.Remove(restaurant1);
            ApplicationDbContext.Remove(city1);
            ApplicationDbContext.Remove(restaurant2);
            ApplicationDbContext.Remove(city2);
            ApplicationDbContext.Remove(restaurant3);
            ApplicationDbContext.Remove(city3);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public void GetById_UnknownIDPassed_ReturnsNotFoundResult()
        {
            var city = new City
            {
                Id = 10,
                Name = "City 10"
            };
            var restaurant = new Restaurant
            {
                Id = 10,
                CityId = 10,
                Name = "Restaurant 10"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();

            var notFoundResult = _restaurantsController.GetRestaurant(11);

            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            ApplicationDbContext.Remove(restaurant);
            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public void GetById_ExistingIDPassed_ReturnsOkResult()
        {
            var city = new City
            {
                Id = 12,
                Name = "City 12"
            };
            var restaurant = new Restaurant
            {
                Id = 12,
                CityId = 12,
                Name = "Restaurant 12"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();

            var okResult = _restaurantsController.GetRestaurant(12);

            Assert.IsType<OkObjectResult>(okResult.Result.Result);

            ApplicationDbContext.Remove(restaurant);
            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public void GetById_ExistingIDPassed_ReturnsRightItem()
        {
            var city = new City
            {
                Id = 13,
                Name = "City 13"
            };
            var restaurant = new Restaurant
            {
                Id = 13,
                CityId = 13,
                Name = "Restaurant 13"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();

            var okResult = _restaurantsController.GetRestaurant(13).Result.Result as OkObjectResult;

            var actionResult = Assert.IsType<ActionResult<RestaurantResponse>>(okResult.Value);
            Assert.Equal(13, (actionResult.Value as RestaurantResponse).Id);

            ApplicationDbContext.Remove(city);
            ApplicationDbContext.Remove(restaurant);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var city = new City
            {
                Id = 14,
                Name = "City 14"
            };
            ApplicationDbContext.Add(city);

            var restaurantRequest = new RestaurantRequest
            {
                Id = 14,
                CityId = 14,
                Name = "Restaurant 14"
            };

            var notFoundResult = _restaurantsController.GetRestaurant(14);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            var createdResponse = _restaurantsController.PostRestaurant(restaurantRequest);
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);

            var okResult = _restaurantsController.GetRestaurant(14).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<RestaurantResponse>>(okResult.Value);
            Assert.Equal(14, (actionResult.Value as RestaurantResponse).Id);

            await _restaurantsController.DeleteRestaurant(14);

            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            var city = new City
            {
                Id = 14,
                Name = "City 14"
            };
            ApplicationDbContext.Add(city);

            var restaurantRequest = new RestaurantRequest
            {
                Id = 14,
                CityId = 14,
                Name = "Restaurant 14"
            };

            var notFoundResult = _restaurantsController.GetRestaurant(14);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            var createdResponse = _restaurantsController.PostRestaurant(restaurantRequest);
            var item = Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);

            var restaurantResponse = Assert.IsType<RestaurantResponse>(item.Value);
            Assert.Equal("Restaurant 14", restaurantResponse.Name);

            var okResult = _restaurantsController.GetRestaurant(14).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<RestaurantResponse>>(okResult.Value);
            Assert.Equal(14, (actionResult.Value as RestaurantResponse).Id);

            await _restaurantsController.DeleteRestaurant(14);

            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Remove_NotExistingIDPassed_ReturnsNotFoundResponse()
        {
            var city = new City
            {
                Id = 15,
                Name = "City 15"
            };
            var restaurant = new Restaurant
            {
                Id = 15,
                CityId = 15,
                Name = "Restaurant 15"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();

            var badResponse = await _restaurantsController.DeleteRestaurant(16);
            Assert.IsType<NotFoundResult>(badResponse.Result);

            ApplicationDbContext.Remove(restaurant);
            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Remove_ExistingIDPassed_ReturnsOkResult()
        {
            var city = new City
            {
                Id = 16,
                Name = "City 16"
            };
            var restaurant = new Restaurant
            {
                Id = 16,
                CityId = 16,
                Name = "Restaurant 16"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();

            var okResult = _restaurantsController.GetRestaurant(16).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<RestaurantResponse>>(okResult.Value);
            Assert.Equal(16, (actionResult.Value as RestaurantResponse).Id);

            var okResponse = await _restaurantsController.DeleteRestaurant(16);
            Assert.IsType<OkObjectResult>(okResponse.Result);

            var notFoundResult = _restaurantsController.GetRestaurant(16);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Update_ValidObjectPassed_ReturnsNoContent()
        {
            var city = new City
            {
                Id = 19,
                Name = "City 19"
            };
            var restaurant = new Restaurant
            {
                Id = 20,
                CityId = 19,
                Name = "Restaurant 20"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant);
            ApplicationDbContext.SaveChanges();
            ApplicationDbContext.Entry(restaurant).State = EntityState.Detached;

            var result = (await _restaurantsController.GetRestaurant(20)).Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<RestaurantResponse>>(result.Value);
            Assert.Equal(20, (actionResult.Value as RestaurantResponse).Id);
            Assert.Equal("Restaurant 20", (actionResult.Value as RestaurantResponse).Name);
           
            var restaurantRequest = new RestaurantRequest
            {
                Id = 20,
                CityId = 19,
                Name = "Restaurant 20 - UPDATED"
            };

            var noContentResponse = await _restaurantsController.PutRestaurant(20, restaurantRequest);
            Assert.IsType<NoContentResult>(noContentResponse);

            var okResult2 = (await _restaurantsController.GetRestaurant(20)).Result as OkObjectResult;
            var actionResult2 = Assert.IsType<ActionResult<RestaurantResponse>>(okResult2.Value);
            Assert.Equal(20, (actionResult2.Value as RestaurantResponse).Id);
            Assert.Equal("Restaurant 20 - UPDATED", (actionResult2.Value as RestaurantResponse).Name);

            await _restaurantsController.DeleteRestaurant(20);
            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }
    }
}
