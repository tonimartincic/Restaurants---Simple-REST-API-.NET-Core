using Microsoft.AspNetCore.Mvc;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Data.EFCore;
using Restaurants.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public class CityService : ICityService
    {
        private readonly CityRepository _cityRepository;

        public CityService(CityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<ActionResult<IEnumerable<CityResponse>>> GetCities()
        {
            var cities =  await _cityRepository.GetAll();

            var cityResponses = MapModelToResponse(cities);
            return cityResponses;
        }

        public async Task<ActionResult<CityResponse>> GetCity(int id)
        {
            var city = await _cityRepository.Get(id);

            var cityResponse = MapModelToResponse(city);
            return cityResponse;
        }

        public async Task<ActionResult<CityResponse>> Update(CityRequest cityRequest)
        {
            var city = MapRequestToModel(cityRequest);

            city = await _cityRepository.Update(city);

            var cityResponse = MapModelToResponse(city);
            return cityResponse;
        }

        public async Task<ActionResult<CityResponse>> Create(CityRequest cityRequest)
        {
            var city = MapRequestToModel(cityRequest);

            city = await _cityRepository.Add(city);

            var cityResponse = MapModelToResponse(city);
            return cityResponse;
        }

        public async Task<ActionResult<CityResponse>> DeleteCity(int id)
        {
            var city = await _cityRepository.Delete(id);

            var cityResponse = MapModelToResponse(city);
            return cityResponse;
        }

        public City MapRequestToModel(CityRequest cityRequest)
        {
            if (cityRequest == null)
            {
                return null;
            }

            var city = new City
            {
                Id = cityRequest.Id,
                Name = cityRequest.Name
            };

            return city;
        }

        public List<City> MapRequestToModel(List<CityRequest> cityRequests)
        {
            if (cityRequests == null)
            {
                return null;
            }

            var cities = new List<City>();
            foreach (var cityRequest in cityRequests)
            {
                var city = MapRequestToModel(cityRequest);
                cities.Add(city);
            }

            return cities;
        }

        public CityResponse MapModelToResponse(City city)
        {
            if (city == null)
            {
                return null;
            }

            var cityResponse = new CityResponse
            {
                Id = city.Id,
                Name = city.Name
            };

            return cityResponse;
        }

        public List<CityResponse> MapModelToResponse(List<City> cities)
        {
            if (cities == null)
            {
                return null;
            }

            var cityResponses = new List<CityResponse>();
            foreach (var city in cities)
            {
                var cityResponse = MapModelToResponse(city);
                cityResponses.Add(cityResponse);
            }

            return cityResponses;
        }
    }
}
