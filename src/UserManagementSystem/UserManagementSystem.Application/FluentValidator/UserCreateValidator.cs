using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using UserManagementSystem.Application.Features.Users.Commands;

namespace UserManagementSystem.Application.FluentValidator
{
    public class UserCreateValidator : AbstractValidator<UserAddCommand>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100);

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(4).WithMessage("Password must be at least 4 characters long.");
        }
    }
}
