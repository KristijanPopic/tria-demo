using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public sealed class CurrentUserService(ICurrentUser currentUser, IUserRepository userRepository)
{
    private User? _authenticatedUser;

    public ICurrentUser CurrentUser => currentUser;

    public async Task<bool> IsAdmin(CancellationToken token = default)
    {
        return await IsInGroup(Group.GroupAdmin, token);
    }

    public async Task<bool> IsInGroup(string groupName, CancellationToken token = default)
    {
        if (!currentUser.IsAuthenticated)
        {
            return false;
        }
        
        _authenticatedUser ??= await userRepository.GetUserByIdAsync(currentUser.UserId, token);
        
        return _authenticatedUser!.Groups.Exists(g => g.GroupName == groupName);
    }
}