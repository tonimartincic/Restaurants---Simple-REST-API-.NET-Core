namespace Restaurants.Controllers.Response
{
    public class RestaurantResponse
    {
        /// <summary>
        /// Restaurant Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Restaurant name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City data.
        /// </summary>
        public virtual CityResponse CityResponse { get; set; }
    }
}
