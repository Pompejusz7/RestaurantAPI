using RestaurantAPI.Models;
using System.Collections.Generic;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int CreateDish(int restaurantId, CreateDishDto dto);
        IEnumerable<DishDto> GetDishes(int restaurantId);
        void DeleteDish(int restaurantId, int dishId);
        DishDto GetDishById(int restaurantId, int dishId);

    }
}