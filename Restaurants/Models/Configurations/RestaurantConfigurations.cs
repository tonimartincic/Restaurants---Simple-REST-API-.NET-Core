using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Restaurants.Models.Configurations
{
    public class RestaurantConfigurations : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurant");

            builder.HasKey("Id");

            builder.HasOne(x => x.City).WithMany(x => x.Restaurants).HasForeignKey(x => x.CityId);
        }
    }
}
