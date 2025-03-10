using TriaDemo.Common;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class UserService(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    IGroupRepository groupRepository
    ) : IUserService
{
    private const string GroupAdmin = "admin";
    
    public async Task<User> CreateUserAsync(User user, CancellationToken token = default)
    {
        var existingUser = await userRepository.GetUserByEmailAsync(user.Email, token);
        if (existingUser is not null)
        {
            throw new ValueNotUniqueException("User with the given email already exists.");
        }
        
        var adminGroup = await groupRepository.GetReaderGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(adminGroup);
        
        return await userRepository.CreateAsync(user, token);
    }

    public async Task<bool> DeleteUserAsync(User user, CancellationToken token = default)
    {
        if (currentUser.UserId == user.Id)
        {
            throw new UnauthorizedException("User can not delete himself.");
        }
        
        var authenticatedUser = await userRepository.GetUserByIdAsync(currentUser.UserId, token);
        if (!authenticatedUser!.Groups.Exists(g => g.GroupName == GroupAdmin))
        {
            throw new UnauthorizedException("User must be admin to delete other users.");
        }
        
        return await userRepository.DeleteAsync(user.Id, token);
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        return await userRepository.GetUserByEmailAsync(email, token);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await userRepository.GetUserByIdAsync(id, token);
    }
}