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
        var readerGroup = await groupRepository.GetReaderGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(readerGroup);
        
        return await userRepository.CreateAsync(user, token);
    }

    public async Task<bool> DeleteUserAsync(User user, CancellationToken token = default)
    {
        if (currentUser.UserId == user.Id)
        {
            throw new UnauthorizedException("User can not delete himself.");
        }
        
        await ThrowIfCurrentUserIsNotAdmin("User must be admin to delete other users.", token);

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

    public async Task<IReadOnlyCollection<User>> GetUsersAsync(CancellationToken token = default)
    {
        return await userRepository.GetUsersAsync(token);
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken token = default)
    {
        await ThrowIfCurrentUserIsNotAdmin("User must be admin to update users.", token);
        
        var groupNames = user.Groups.Select(g => g.GroupName).ToArray();
        var groups = await groupRepository.GetGroupsAsync(groupNames, token);

        user.Groups = groups.ToList();

        return await userRepository.UpdateAsync(user, token);
    }

    private async Task ThrowIfCurrentUserIsNotAdmin(string errorMessage, CancellationToken token)
    {
        var authenticatedUser = await userRepository.GetUserByIdAsync(currentUser.UserId, token);
        if (!authenticatedUser!.Groups.Exists(g => g.GroupName == GroupAdmin))
        {
            throw new UnauthorizedException(errorMessage);
        }
    }
}