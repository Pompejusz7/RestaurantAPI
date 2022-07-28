using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public RestaurantDto GetById(int id)
        {
            var resturant = _context
              .Restaurants
              .Include(r => r.Address)
              .Include(r => r.Dishes)
              .FirstOrDefault(x => x.Id == id);

            if (resturant == null) throw new NotFoundException("Restaurant not found");

            var resturantDto = _mapper.Map<RestaurantDto>(resturant);

            return resturantDto;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var resturants = _context
              .Restaurants
              .Include(r => r.Address)
              .Include(r => r.Dishes)
              .ToList();

            var resturantsDtos = _mapper.Map<List<RestaurantDto>>(resturants);

            return resturantsDtos;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var newRestaurant = _mapper.Map<Restaurant>(dto);

            _context.Restaurants.Add(newRestaurant);
            _context.SaveChanges();

            return newRestaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action Invoked");
            var restaurantToDelete = _context.Restaurants.SingleOrDefault(x => x.Id == id);

            if(restaurantToDelete == null) throw new NotFoundException("Restaurant not found");

            _context.Restaurants.Remove(restaurantToDelete);
            _context.SaveChanges();
        }

        public void UpdateRestaurant(UpdateRestaurantDto updateRestaurantDto, int id)
        {
            var restaurant = _context.Restaurants.SingleOrDefault(x => x.Id == id);

            if (restaurant == null) throw new NotFoundException("Restaurant not found");

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            if(updateRestaurantDto.dishes != null)
                restaurant.Dishes = _mapper.Map<List<Dish>>(updateRestaurantDto.dishes);

            _context.SaveChanges();
        }
    }
}
