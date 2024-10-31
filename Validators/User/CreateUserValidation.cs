using Concessionaria.Models.Users;
using FluentValidation;
using System.Linq;

namespace Concessionaria.Validators.User;

public class CreateUserValidation : AbstractValidator<UserCreateDTO>
{
    public CreateUserValidation()
        {
        RuleFor(user => user.Name).NotEmpty();

        RuleFor(user => user.LastName).NotEmpty();

        RuleFor(user => user.Login).NotEmpty();

        RuleFor(user => user.Password).NotEmpty();

        RuleFor(user => user.Role).NotEmpty();
    }
}
