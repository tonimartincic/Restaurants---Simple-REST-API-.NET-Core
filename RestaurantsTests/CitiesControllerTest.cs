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
    public class CitiesControllerTest
    {
        public ApplicationDbContext ApplicationDbContext { get; set; }

        private CitiesController _citiesController;

        private IRestaurantService _restaurantService;

        private ICityService _cityService;

        private RestaurantRepository _restaurantRepository;

        private CityRepository _cityRepository;

        public CitiesControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Cities-testdb")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .Options;

            ApplicationDbContext = new ApplicationDbContext(options);

            _cityRepository = new CityRepository(ApplicationDbContext);
            _restaurantRepository = new RestaurantRepository(ApplicationDbContext);

            _cityService = new CityService(_cityRepository);
            _restaurantService = new RestaurantService(_restaurantRepository, _cityService);

            _citiesController = new CitiesController(_cityService, _restaurantService);
        }

        [Fact]
        public void Get_WhenCalled_ReturnsOkResult()
        {
            var result = _citiesController.GetCities();

            Assert.IsType<OkObjectResult>(result.Result.Result);
        }


        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            var city1 = new City
            {
                Id = 1,
                Name = "City 1"
            };
            var city2 = new City
            {
                Id = 2,
                Name = "City 2"
            };
            var city3 = new City
            {
                Id = 3,
                Name = "City 3"
            };
            ApplicationDbContext.Add(city1);
            ApplicationDbContext.Add(city2);
            ApplicationDbContext.Add(city3);
            ApplicationDbContext.SaveChanges();

            var result = _citiesController.GetCities();

            var okResult = Assert.IsType<OkObjectResult>(result.Result.Result);

            var items = Assert.IsType<ActionResult<IEnumerable<CityResponse>>>(okResult.Value);

            var cityResponses = items.Value;

            int numberOfCities = 0;
            using (IEnumerator<CityResponse> enumerator = cityResponses.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    numberOfCities++;
                }
            }

            Assert.Equal(3, numberOfCities);

            ApplicationDbContext.Remove(city1);
            ApplicationDbContext.Remove(city2);
            ApplicationDbContext.Remove(city3);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public void GetById_UnknownIDPassed_ReturnsNotFoundResult()
        {
            var city1 = new City
            {
                Id = 1,
                Name = "City 1"
            };
            var city2 = new City
            {
                Id = 2,
                Name = "City 2"
            };
            var city3 = new City
            {
                Id = 3,
                Name = "City 3"
            };
            ApplicationDbContext.Add(city1);
            ApplicationDbContext.Add(city2);
            ApplicationDbContext.Add(city3);
            ApplicationDbContext.SaveChanges();

            var notFoundResult = _citiesController.GetCity(11);

            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            ApplicationDbContext.Remove(city1);
            ApplicationDbContext.Remove(city2);
            ApplicationDbContext.Remove(city3);
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
            ApplicationDbContext.Add(city);
            ApplicationDbContext.SaveChanges();

            // Act
            var okResult = _citiesController.GetCity(12);

            // Assert
            Assert.IsType<OkObjectResult>(okResult.Result.Result);

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
            ApplicationDbContext.Add(city);
            ApplicationDbContext.SaveChanges();

            // Act
            var okResult = _citiesController.GetCity(13).Result.Result as OkObjectResult;

            // Assert
            var actionResult = Assert.IsType<ActionResult<CityResponse>>(okResult.Value);
            Assert.Equal(13, (actionResult.Value as CityResponse).Id);

            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }

        [Fact]
        public async void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            var cityRequest = new CityRequest
            {
                Id = 14,
                Name = "City 14"
            };

            var notFoundResult = _citiesController.GetCity(14);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            var createdResponse = _citiesController.PostCity(cityRequest);
            Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);

            var okResult = _citiesController.GetCity(14).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<CityResponse>>(okResult.Value);
            Assert.Equal(14, (actionResult.Value as CityResponse).Id);

            await _citiesController.DeleteCity(14);
        }

        [Fact]
        public async void Add_ValidObjectPassed_ReturnedResponseHasCreatedItem()
        {
            var cityRequest = new CityRequest
            {
                Id = 14,
                Name = "City 14"
            };

            var notFoundResult = _citiesController.GetCity(14);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);

            var createdResponse = _citiesController.PostCity(cityRequest);
            var item = Assert.IsType<CreatedAtActionResult>(createdResponse.Result.Result);

            var cityResponse = Assert.IsType<CityResponse>(item.Value);
            Assert.Equal("City 14", cityResponse.Name);

            var okResult = _citiesController.GetCity(14).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<CityResponse>>(okResult.Value);
            Assert.Equal(14, (actionResult.Value as CityResponse).Id);

            await _citiesController.DeleteCity(14);
        }

        [Fact]
        public async void Remove_NotExistingIDPassed_ReturnsNotFoundResponse()
        {
            var city = new City
            {
                Id = 15,
                Name = "City 15"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.SaveChanges();

            var badResponse = await _citiesController.DeleteCity(16);
            Assert.IsType<NotFoundResult>(badResponse.Result);

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
            ApplicationDbContext.Add(city);
            ApplicationDbContext.SaveChanges();

            var okResult = _citiesController.GetCity(16).Result.Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<CityResponse>>(okResult.Value);
            Assert.Equal(16, (actionResult.Value as CityResponse).Id);

            var okResponse = await _citiesController.DeleteCity(16);
            Assert.IsType<OkObjectResult>(okResponse.Result);

            var notFoundResult = _citiesController.GetCity(16);
            Assert.IsType<NotFoundResult>(notFoundResult.Result.Result);
        }

        [Fact]
        public async void Update_ValidObjectPassed_ReturnsNoContent()
        {
            var city = new City
            {
                Id = 17,
                Name = "City 17"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.SaveChanges();
            ApplicationDbContext.Entry(city).State = EntityState.Detached;
            
            var result = (await _citiesController.GetCity(17)).Result as OkObjectResult;
            var actionResult = Assert.IsType<ActionResult<CityResponse>>(result.Value);
            Assert.Equal(17, (actionResult.Value as CityResponse).Id);
            Assert.Equal("City 17", (actionResult.Value as CityResponse).Name);

            var cityRequest = new CityRequest
            {
                Id = 17,
                Name = "City 17 - UPDATED"
            };

            var noContentResponse = await _citiesController.PutCity(17, cityRequest);
            Assert.IsType<NoContentResult>(noContentResponse);

            var okResult2 = (await _citiesController.GetCity(17)).Result as OkObjectResult;
            var actionResult2 = Assert.IsType<ActionResult<CityResponse>>(okResult2.Value);
            Assert.Equal(17, (actionResult2.Value as CityResponse).Id);
            Assert.Equal("City 17 - UPDATED", (actionResult2.Value as CityResponse).Name);

            await _citiesController.DeleteCity(17);
        }

        [Fact]
        public async void GetRestaurantsForCity()
        {
            var city = new City
            {
                Id = 100,
                Name = "City 100"
            };
            var restaurant101 = new Restaurant
            {
                Id = 101,
                CityId = 100,
                Name = "Restaurant 101"
            };
            var restaurant102 = new Restaurant
            {
                Id = 102,
                CityId = 100,
                Name = "Restaurant 102"
            };
            var restaurant103 = new Restaurant
            {
                Id = 103,
                CityId = 100,
                Name = "Restaurant 103"
            };
            var restaurant104 = new Restaurant
            {
                Id = 104,
                CityId = 100,
                Name = "Restaurant 104"
            };
            ApplicationDbContext.Add(city);
            ApplicationDbContext.Add(restaurant101);
            ApplicationDbContext.Add(restaurant102);
            ApplicationDbContext.Add(restaurant103);
            ApplicationDbContext.Add(restaurant104);
            ApplicationDbContext.SaveChanges();

            var result = (await _citiesController.GetRestaurantsForCity(100)).Result;
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Assert
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

            Assert.Equal(4, numberOfRestaurants);

            ApplicationDbContext.Remove(restaurant101);
            ApplicationDbContext.Remove(restaurant102);
            ApplicationDbContext.Remove(restaurant103);
            ApplicationDbContext.Remove(restaurant104);
            ApplicationDbContext.Remove(city);
            ApplicationDbContext.SaveChanges();
        }
    }
}
