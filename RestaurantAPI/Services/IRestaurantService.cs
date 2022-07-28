using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int CreateRestaurant(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        bool DeleteRestaurant(int id);

        bool UpdateRestaurant(UpdateRestaurantDto updateRestaurantDto, int id);
    }
}