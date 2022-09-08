using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                if (_dbContext.Database.IsRelational())
                {
                    var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                    if (pendingMigrations != null && pendingMigrations.Any())
                    {
                        _dbContext.Database.Migrate();
                    }
                }

                if (!_dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _dbContext.Restaurants.AddRange(restaurants);
                    _dbContext.SaveChanges();
                }


                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }
            
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "Kentucky Fried Chicken",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Strips 5 piece",
                            Price = 5.30M
                        },
                        new Dish()
                        {
                            Name = "Bites 100g",
                            Price = 7.40M
                        }

                    },
                    Address = new Address()
                    {
                        City = "Katowice",
                        Street = "Mariacka 4",
                        PostalCode = "43-100"
                    }
                },
                new Restaurant()
                {
                     Name = "McDonalds",
                     Category = "Fast Food",
                     Description = "Burgers",
                     ContactEmail = "contact@mcd.com",
                     HasDelivery = true,
                     Dishes = new List<Dish>()
                        {
                            new Dish()
                            {
                                Name = "Big MAC",
                                Price = 3.30M
                            },
                            new Dish()
                            {
                                Name = "CheeseBurger",
                                Price = 2M
                            }

                        },
                     Address = new Address()
                     {
                         City = "Katowice",
                         Street = "Mariacka 7",
                         PostalCode = "43-100"
                     }
                }
            };

            return restaurants;
        }
    }
}
