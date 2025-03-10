using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface IUserService
{
    Task<User> CreateUserAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteUserAsync(User user, CancellationToken token = default);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default);
    
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IReadOnlyCollection<User>> GetUsersAsync(CancellationToken token = default);
}