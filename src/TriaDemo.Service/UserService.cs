using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class UserService(
    CurrentUserService currentUserService,
    IUserRepository userRepository,
    IGroupRepository groupRepository
    ) : IUserService
{
    public async Task<User> CreateUserAsync(User user, CancellationToken token = default)
    {
        var readerGroup = await groupRepository.GetReaderGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(readerGroup);
        
        return await userRepository.CreateAsync(user, token);
    }

    public async Task<bool> DeleteUserAsync(User user, CancellationToken token = default)
    {
        if (currentUserService.CurrentUser.UserId == user.Id)
        {
            throw new UnauthorizedException("User can not delete himself.");
        }
        
        if (!await currentUserService.IsAdmin(token))
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

    public async Task<IReadOnlyCollection<User>> GetUsersAsync(CancellationToken token = default)
    {
        return await userRepository.GetUsersAsync(token);
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken token = default)
    {
        if (!await currentUserService.IsAdmin(token))
        {
            throw new UnauthorizedException("User must be admin to update users.");
        }

        var groupNames = user.Groups.Select(g => g.GroupName).ToArray();
        var groups = await groupRepository.GetGroupsAsync(groupNames, token);
        
        if (groups.Count == 0)
        {
            throw new InvalidEntityException("User must be assigned to at least one group.");
        }

        user.Groups = groups.ToList();

        return await userRepository.UpdateAsync(user, token);
    }
}