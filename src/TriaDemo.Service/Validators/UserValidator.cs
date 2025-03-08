using FluentValidation;
using TriaDemo.Service.Models;

namespace TriaDemo.Service.Validators;

internal sealed class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
        RuleFor(u => u.PasswordHash).NotEmpty();
    }
}