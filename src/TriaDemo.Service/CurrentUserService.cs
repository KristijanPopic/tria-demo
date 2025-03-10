using TriaDemo.Common;
using TriaDemo.Service.Contracts;

namespace TriaDemo.Service;

internal sealed class CurrentUserService(ICurrentUser currentUser, IUserRepository userRepository)
{
    private const string GroupAdmin = "admin";
    
    private bool? _isAdmin;

    public ICurrentUser CurrentUser => currentUser;

    public async Task<bool> IsAdmin(CancellationToken token = default)
    {
        if (!currentUser.IsAuthenticated)
        {
            return false;
        }
        
        if (!_isAdmin.HasValue)
        {
            var authenticatedUser = await userRepository.GetUserByIdAsync(currentUser.UserId, token);
            _isAdmin = authenticatedUser!.Groups.Exists(g => g.GroupName == GroupAdmin);
        }
        
        return _isAdmin.Value;
    }
}