using FluentValidation;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class UserService(
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    IValidator<User> validator
    ) : IUserService
{
    public async Task<User> CreateUserAsync(User user, CancellationToken token = default)
    {
        await validator.ValidateAndThrowAsync(user, token);
        
        var existingUser = await userRepository.GetUserByEmailAsync(user.Email, token);

        if (existingUser != null)
        {
            throw new ValueNotUniqueException("User with the given email already exists.");
        }
        
        var adminGroup = await groupRepository.GetReaderGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(adminGroup);
        
        return await userRepository.CreateAsync(user, token);
    }
}