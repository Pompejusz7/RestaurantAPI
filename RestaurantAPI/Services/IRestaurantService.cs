using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int CreateRestaurant(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);
        void DeleteRestaurant(int id);

        void UpdateRestaurant(UpdateRestaurantDto updateRestaurantDto, int id);
    }
}