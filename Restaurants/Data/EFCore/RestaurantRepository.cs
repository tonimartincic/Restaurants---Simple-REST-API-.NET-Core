using Microsoft.EntityFrameworkCore;
using Restaurants.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Data.EFCore
{
    public class RestaurantRepository : EfCoreRepository<Restaurant, ApplicationDbContext>
    {
        private readonly ApplicationDbContext _context;

        public RestaurantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Restaurant> GetRestaurantsForCity(int id)
        {
            return _context.Restaurants
                .Include(x => x.City)
                .Where(x => x.CityId == id)
                .ToList();
        }

        public new async Task<Restaurant> Get(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(x => x.City)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            return restaurant;
        }

        public new async Task<List<Restaurant>> GetAll()
        {
            var restaurants = await _context.Restaurants
                .Include(x => x.City)
                .ToListAsync();

            return restaurants;
        }
    }
}
