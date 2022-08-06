using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext context, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
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

        public async Task<IEnumerable<RestaurantDto>> GetAll()
        {
            var resturants = await _context
              .Restaurants
              .Include(r => r.Address)
              .Include(r => r.Dishes)
              .ToListAsync();

            var resturantsDtos = _mapper.Map<List<RestaurantDto>>(resturants);

            return resturantsDtos;
        }

        public int CreateRestaurant(CreateRestaurantDto dto)
        {
            var newRestaurant = _mapper.Map<Restaurant>(dto);
            newRestaurant.CreatedById = _userContextService.GetUserId;

            _context.Restaurants.Add(newRestaurant);
            _context.SaveChanges();

            return newRestaurant.Id;
        }

        public void DeleteRestaurant(int id)
        {
            _logger.LogInformation($"Restaurant with id: {id} DELETE action Invoked");
            var restaurantToDelete = _context.Restaurants.SingleOrDefault(x => x.Id == id);

            if(restaurantToDelete == null) throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurantToDelete, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException();

            _context.Restaurants.Remove(restaurantToDelete);
            _context.SaveChanges();
        }

        public void UpdateRestaurant(UpdateRestaurantDto updateRestaurantDto, int id)
        {
            var restaurant = _context.Restaurants.SingleOrDefault(x => x.Id == id);

            if (restaurant == null) throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded) throw new ForbidException();

            restaurant.Name = updateRestaurantDto.Name;
            restaurant.Description = updateRestaurantDto.Description;
            restaurant.HasDelivery = updateRestaurantDto.HasDelivery;

            if(updateRestaurantDto.dishes != null)
                restaurant.Dishes = _mapper.Map<List<Dish>>(updateRestaurantDto.dishes);

            _context.SaveChanges();
        }
    }
}
