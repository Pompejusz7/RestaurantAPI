using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Models
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ContactEmail { get; set; }
        public bool HasDelivery { get; set; }
        [Required]
        [MaxLength(25)]
        public string City { get; set; }
        [Required]
        [MaxLength(25)]
        public string Street { get; set; }
        public string PostalCode { get; set; }

        public List<DishDto>Dishes { get; set; }
    }
}
