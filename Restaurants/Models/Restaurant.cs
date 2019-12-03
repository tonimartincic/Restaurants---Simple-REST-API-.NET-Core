using Restaurants.Data;

namespace Restaurants.Models
{
    public class Restaurant : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int CityId { get; set; }

        public virtual City City { get; set; }
    }
}
