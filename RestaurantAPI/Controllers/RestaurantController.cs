using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {

        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService=restaurantService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Restaurant>> GetAll()
        {
         
            var resturantsDtos =_restaurantService.GetAll();

            return Ok(resturantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<Restaurant>> Get([FromRoute]int id)
        {
            var resturantDto = _restaurantService.GetById(id);

            return Ok(resturantDto);
        }

        [HttpPost]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            int addedId = _restaurantService.CreateRestaurant(dto);

            return Created($"/api/restaurant/{addedId}", null);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRestaurant([FromRoute]int id)
        {
            _restaurantService.DeleteRestaurant(id);

            return NoContent();
        }

        [HttpPut("{id}")]
        public ActionResult PutRestaurant([FromRoute] int id, [FromBody]UpdateRestaurantDto restaurantDto)
        {
            _restaurantService.UpdateRestaurant(restaurantDto, id);

            return Ok();
        }
    }
}
