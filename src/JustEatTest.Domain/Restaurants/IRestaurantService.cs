using System.Collections.Generic;
using System.Threading.Tasks;
using JustEatTest.Domain.Restaurants.Model;

namespace JustEatTest.Domain.Restaurants
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetByPostcodeAsync(string postcode);
    }
}
