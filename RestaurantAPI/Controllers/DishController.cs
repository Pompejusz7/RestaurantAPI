using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Collections.Generic;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant/{restaurantId}/dish")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _service;

        public DishController(IDishService service)
        {
            _service = service;
        }

        [HttpPost]
        public ActionResult CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
        {
            int newDishId = _service.CreateDish(restaurantId, dto);

            return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
        }

        [HttpGet]
        public ActionResult<List<DishDto>> GetAll([FromRoute] int restaurantId)
        {
           var dishesDto = _service.GetDishes(restaurantId);

            return Ok(dishesDto);
        }

        [HttpGet("{dishId}")]
        public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
            var dishDto = _service.GetDishById(restaurantId, dishId);

            return Ok(dishDto);
        }

        [HttpDelete("{dishId}")]
        public ActionResult<List<DishDto>> DeleteDish([FromRoute] int restaurantId, [FromRoute] int dishId)
        {
           _service.DeleteDish(restaurantId, dishId);

            return NoContent();
        }

    }
}
