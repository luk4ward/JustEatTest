using System.Collections.Generic;

namespace JustEatTest.Domain.Restaurants.Model.JustEat
{
    public class RestaurantResponse
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
    }
}