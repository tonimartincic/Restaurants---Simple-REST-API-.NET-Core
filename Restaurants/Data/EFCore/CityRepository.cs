using Restaurants.Models;

namespace Restaurants.Data.EFCore
{
    public class CityRepository : EfCoreRepository<City, ApplicationDbContext>
    {
        public CityRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
