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
        var regularGroup = await groupRepository.GetRegularGroupAsync(token);
        
        user.IsActive = true;
        user.Groups.Add(regularGroup);
        
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
        return await userRepository.GetByEmailAsync(email, token);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await userRepository.GetByIdAsync(id, token);
    }

    public async Task<IReadOnlyCollection<User>> GetAsync(CancellationToken token = default)
    {
        return await userRepository.GetAsync(token);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken token = default)
    {
        if (!await currentUserService.IsAdmin(token))
        {
            throw new UnauthorizedException("User must be admin to update users.");
        }

        var groupNames = user.Groups.Select(g => g.GroupName).ToArray();
        var groups = await groupRepository.GetAsync(groupNames, token);

        user.Groups = groups.ToList();

        return await userRepository.UpdateAsync(user, token);
    }
}