using Microsoft.AspNetCore.Mvc;
using Restaurants.Controllers.Request;
using Restaurants.Controllers.Response;
using Restaurants.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Restaurants.Services
{
    public interface ICityService
    {
        Task<ActionResult<IEnumerable<CityResponse>>> GetCities();

        Task<ActionResult<CityResponse>> GetCity(int id);

        Task<ActionResult<CityResponse>> Update(CityRequest cityRequest);

        Task<ActionResult<CityResponse>> Create(CityRequest cityRequest);

        Task<ActionResult<CityResponse>> DeleteCity(int id);

        City MapRequestToModel(CityRequest cityRequest);

        List<City> MapRequestToModel(List<CityRequest> cityRequests);

        CityResponse MapModelToResponse(City city);

        List<CityResponse> MapModelToResponse(List<City> cities);
    }
}
