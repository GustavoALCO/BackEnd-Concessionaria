using Concessionaria.Models.Users;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Concessionaria.Validators.User;

public class AlterationUserValidation : AbstractValidator<UserAlterationDTO>
{

    public AlterationUserValidation()
    {
        RuleFor(user => user.Name).NotEmpty();

        RuleFor(user => user.LastName).NotEmpty();

        RuleFor(user => user.Role).NotEmpty();
    }
}
