using Restaurants.Data;
using System.Collections.Generic;

namespace Restaurants.Models
{
    public class City : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }

        public City()
        {
            Restaurants = new HashSet<Restaurant>();
        }
    }
}
