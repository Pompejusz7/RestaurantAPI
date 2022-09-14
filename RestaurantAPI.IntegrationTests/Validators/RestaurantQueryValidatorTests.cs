using FluentValidation.TestHelper;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Models.Validators;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace RestaurantAPI.IntegrationTests.Validators
{
    public class RestaurantQueryValidatorTests
    {
        public static IEnumerable<object[]> GetSampleValidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 5
                },
                new RestaurantQuery()
                {
                    PageNumber = 2,
                    PageSize = 15
                },
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.Name)
                },
                new RestaurantQuery()
                {
                    PageNumber = 12,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.Category)
                }
            };

            return list.Select( x=> new object[] { x });
        }

        public static IEnumerable<object[]> GetSampleInvalidData()
        {
            var list = new List<RestaurantQuery>()
            {
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 2
                },
                new RestaurantQuery()
                {
                    PageNumber = 2,
                    PageSize = 13
                },
                new RestaurantQuery()
                {
                    PageNumber = 1,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.ContactEmail)
                },
                new RestaurantQuery()
                {
                    PageNumber = 12,
                    PageSize = 5,
                    SortBy = nameof(Restaurant.ContactNumber)
                }
            };

            return list.Select(x => new object[] { x });
        }


        [Theory]
        [MemberData(nameof(GetSampleValidData))]
        public void Validate_ForCorrectModel_ReturnsSuccess(RestaurantQuery model)
        {
            //arrange
            var validator = new RestaurantQueryValidator();

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldNotHaveAnyValidationErrors();

        }

        [Theory]
        [MemberData(nameof(GetSampleInvalidData))]
        public void Validate_ForIncorrectModel_ReturnsFailure(RestaurantQuery model)
        {
            //arrange
            var validator = new RestaurantQueryValidator();

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldHaveAnyValidationError();

        }

    }
}
