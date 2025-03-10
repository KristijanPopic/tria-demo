using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteAsync(Guid userId, CancellationToken token = default);
    
    Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default);
    
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IReadOnlyCollection<User>> GetUsersAsync(CancellationToken token = default);
    
    Task<User> UpdateAsync(User user, CancellationToken token = default);
}