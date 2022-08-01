using FluentValidation;
using RestaurantAPI.Entities;
using System.Linq;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator(RestaurantDbContext dbContext)
        {

            RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

            RuleFor(x => x.Password).MinimumLength(6);

            RuleFor(x => x.ConfirmPassword).Equal(c => c.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    var any = dbContext.Users.Any(x => x.Email == value);
                    if(any)
                    {
                        context.AddFailure("Email", "Email already in use");
                    }
                });
        }
    }
}
