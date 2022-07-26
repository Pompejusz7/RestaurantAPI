using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public RestaurantController(RestaurantDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
            var resturants = _context
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .ToList();

            //var resturantsDtos = resturant.Select(r => new RestaurantDto()
            //{ 
            //    Name = r.Name,
            //    Category = r.Category, 
            //    City = r.Address.City
            //});

            var resturantsDtos = _mapper.Map<List<RestaurantDto>>(resturants);

            return Ok(resturantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Restaurant>> Get([FromRoute]int id)
        {
            var resturant = _context
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(x => x.Id == id);

            var resturantz = _context
                .Restaurants
                .FirstOrDefault(x => x.Id == id);

            var dd = resturantz.Address;

            if (resturant == null) return NotFound();

            var resturantDto = _mapper.Map<RestaurantDto>(resturant);

            return Ok(resturantDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var newRestaurant = _mapper.Map<Restaurant>(dto);

            _context.Restaurants.Add(newRestaurant);
            _context.SaveChanges();

            return Created($"/api/restaurant/{newRestaurant.Id}", null);
        }
    }
}
