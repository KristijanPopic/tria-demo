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
    public async Task<User> CreateAsync(User user, CancellationToken token = default)
    {
        var readerGroup = await groupRepository.GetReaderGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(readerGroup);
        
        return await userRepository.CreateAsync(user, token);
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken token = default)
    {
        if (!await currentUserService.IsAdmin(token))
        {
            throw new UnauthorizedException("User must be admin to delete other users.");
        }
        if (currentUserService.CurrentUser.UserId == user.Id)
        {
            throw new UnauthorizedException("User can not delete himself.");
        }

        return await userRepository.DeleteAsync(user.Id, token);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    {
        return await userRepository.GetUserByEmailAsync(email, token);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await userRepository.GetUserByIdAsync(id, token);
    }

    public async Task<IReadOnlyCollection<User>> GetAsync(CancellationToken token = default)
    {
        return await userRepository.GetUsersAsync(token);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken token = default)
    {
        if (!await currentUserService.IsAdmin(token))
        {
            throw new UnauthorizedException("User must be admin to update users.");
        }

        var groupNames = user.Groups.Select(g => g.GroupName).ToArray();
        var groups = await groupRepository.GetAsync(groupNames, token);
        
        if (groups.Count == 0)
        {
            throw new InvalidEntityException("User must be assigned to at least one group.");
        }

        user.Groups = groups.ToList();

        return await userRepository.UpdateAsync(user, token);
    }
}