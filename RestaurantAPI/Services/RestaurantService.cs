using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
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
        public RestaurantService(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public RestaurantDto GetById(int id)
        {
            var resturant = _context
              .Restaurants
              .Include(r => r.Address)
              .Include(r => r.Dishes)
              .FirstOrDefault(x => x.Id == id);

            if (resturant == null) return null;

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

        public bool DeleteRestaurant(int id)
        {
            var restaurantToDelete = _context.Restaurants.SingleOrDefault(x => x.Id == id);

            if (restaurantToDelete == null) return false;
            

            _context.Restaurants.Remove(restaurantToDelete);
            _context.SaveChanges();

            return true;
        }
    }
}
