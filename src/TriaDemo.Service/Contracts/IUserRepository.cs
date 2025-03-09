using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteAsync(User user, CancellationToken token = default);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default);
}