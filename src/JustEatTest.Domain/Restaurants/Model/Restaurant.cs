using System.Collections.Generic;
using Newtonsoft.Json;

namespace JustEatTest.Domain.Restaurants.Model
{
    public class Restaurant
    {
        public string Name { get; set; }
        public float RatingAverage { get; set; }
        public IEnumerable<Cuisine> CuisineTypes { get; set; }
    }
}