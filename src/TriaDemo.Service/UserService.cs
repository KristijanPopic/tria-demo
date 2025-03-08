using FluentValidation;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class UserService(IUserRepository userRepository, IValidator<User> validator) : IUserService
{
    public async Task<User> CreateUserAsync(User user, CancellationToken token = default)
    {
        await validator.ValidateAndThrowAsync(user, token);
        
        user.IsActive = true;
        
        return await userRepository.CreateAsync(user, token);
    }
}