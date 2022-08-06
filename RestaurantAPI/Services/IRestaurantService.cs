using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IRestaurantService
    {
        int CreateRestaurant(CreateRestaurantDto dto);
        Task<IEnumerable<RestaurantDto>> GetAll();
        RestaurantDto GetById(int id);
        void DeleteRestaurant(int id);
        void UpdateRestaurant(UpdateRestaurantDto updateRestaurantDto, int id);
    }
}