using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RestaurantAPI.Entities;
using RestaurantAPI.IntegrationTests.Helpers;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RestaurantAPI.IntegrationTests
{
    public class AccountControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Startup> _factory;
        private Mock<IAccountService> _accountserviceMock = new Mock<IAccountService>();
        public AccountControllerTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<RestaurantDbContext>));
                            services.Remove(dbContextOptions);

                            services.AddSingleton<IAccountService>(_accountserviceMock.Object);

                            services.AddDbContext<RestaurantDbContext>(options => options.UseInMemoryDatabase("InMem"));
                        });
                    });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsOk()
        {
            //arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "asd@sd.com",
                Password = "password123",
                ConfirmPassword = "password123"

            };

            var httpContent = registerUser.ToJsonHttpContent();

            //act

            var response = await _client.PostAsync("/api/account/register", httpContent);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task RegisterUser_ForValidModel_ReturnsBadRequest()
        {
            //arrange

            var registerUser = new RegisterUserDto()
            {
                Email = "asd@sd.com",
                Password = "passwofrd123",
                ConfirmPassword = "password123"

            };

            var httpContent = registerUser.ToJsonHttpContent();

            //act

            var response = await _client.PostAsync("/api/account/register", httpContent);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnsOk()
        {
            //arrange
            _accountserviceMock
                .Setup(e => e.GenerateJwt(It.IsAny<LoginDto>()))
                .Returns("jwt");

            var loginDto = new LoginDto()
            {
                Email = "test@test.pl",
                Password = "password123"
            };

            var httpContent = loginDto.ToJsonHttpContent();

            //act
            var response = await _client.PostAsync("/api/account/login", httpContent);


            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
