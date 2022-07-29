using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public class DishService : IDishService
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _context;

        public DishService(IMapper mapper, RestaurantDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public int CreateDish(int restaurantId, CreateDishDto dto)
        {
            var restaurant = _context.Restaurants.SingleOrDefault(x => x.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException("Not found restaurant id");

            dto.RestaurantId = restaurantId;

            var newDish = _mapper.Map<Dish>(dto);

            _context.Dishes.Add(newDish);
            _context.SaveChanges();

            return newDish.Id;
        }

        public IEnumerable<DishDto> GetDishes(int restaurantId)
        {
            var restaurant = _context.Restaurants.SingleOrDefault(x => x.Id == restaurantId);

            if (restaurant == null) throw new NotFoundException("Not found restaurant id");

            var dishes = _context.Dishes.Where(x => x.RestaurantId == restaurantId).ToList();

            var dishesDtos = _mapper.Map<List<DishDto>>(dishes);

            return dishesDtos;
        }

        public DishDto GetDishById(int restaurantId, int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.Id == dishId && x.RestaurantId == restaurantId);

            if (dish == null) throw new NotFoundException("Not found restaurant id or dish id");

            var dishDto = _mapper.Map<DishDto>(dish);

            return dishDto;
        }
        
        public void DeleteDish(int restaurantId, int dishId)
        {
            var dish = _context.Dishes.SingleOrDefault(x => x.Id == dishId && x.RestaurantId == restaurantId);

            if (dish == null) throw new NotFoundException("Not found restaurant id or dish id");

            _context.Dishes.Remove(dish);

            _context.SaveChanges();
        }
    }
}
