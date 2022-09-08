using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using FluentAssertions;
using System.Net.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using Microsoft.Extensions.DependencyInjection;
using RestaurantAPI.Models;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization.Policy;
using RestaurantAPI.IntegrationTests.Helpers;

namespace RestaurantAPI.IntegrationTests
{
    public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;
        public RestaurantControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                        services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("InMem"));
                    });
                });

            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("pageSize=5&pageNumber=1")]
        [InlineData("pageSize=15&pageNumber=2")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {
            //arrange

            //act

            var response = await _client.GetAsync("api/restaurant?" + queryParams);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("pageSize=11&pageNumber=1")]
        [InlineData("pageSize=100&pageNumber=2")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAll_WithInvalidQueryParams_ReturnsBadRequest(string queryParams)
        {
            //arrange

            //act

            var response = await _client.GetAsync("api/restaurant?" + queryParams);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateRestaurant_WithValidModel_ReturnsCreatedStatus()
        {
            //arrange
            var model = new CreateRestaurantDto()
            {
                Name = "TestRestaurant",
                City = "Hamburg",
                Street = "Krossseee"
            };


            var httpContent = model.ToJsonHttpContent();

            //act
            var response = await _client.PostAsync("api/restaurant", httpContent);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

        }

        [Fact]
        public async Task CreateRestaurant_WithInvalidModel_ReturnsBadRequest()
        {
            //arrange
            var model = new CreateRestaurantDto()
            {
                ContactEmail = "wer@we.com",
                Description = "asd",
                ContactNumber = "23 1233"
            };

            var httpContent = model.ToJsonHttpContent();

            //act
            var response = await _client.PostAsync("api/restaurant", httpContent);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_ForNoNExistingRestaurant_ReturnsNotFound()
        {
            //act

            var response = await _client.DeleteAsync("/api/restaurant/9009");

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ForRestaurantOwner_ReturnsNoContent()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                //bec. fake user claim id = 1
                CreatedById = 1,
                Name= "test"
            };

            SeedRestaurant(restaurant);

            //act

            var response = await _client.DeleteAsync("/api/restaurant/" + restaurant.Id);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task Delete_ForNonRestaurantOwner_ReturnsForbidden()
        {
            //arrange

            var restaurant = new Restaurant()
            {
                //bec. fake user claim id = 1
                CreatedById = 900,
                Name = "test"
            };

            SeedRestaurant(restaurant);

            //act

            var response = await _client.DeleteAsync("/api/restaurant/" + restaurant.Id);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        private void SeedRestaurant(Restaurant restaurant)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

            using (var scope = scopeFactory.CreateScope())
            {
                var _dbcontext = scope.ServiceProvider.GetService<RestaurantDbContext>();
                _dbcontext.Restaurants.Add(restaurant);
                _dbcontext.SaveChanges();
            }
        }
    }
}
