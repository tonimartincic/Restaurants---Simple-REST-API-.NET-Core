namespace Restaurants.Controllers.Request
{
    public class RestaurantRequest
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
        /// City Id.
        /// </summary>
        public int CityId { get; set; }
    }
}
