using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface IUserService
{
    Task<User> CreateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteAsync(User user, CancellationToken token = default);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken token = default);
    
    Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IReadOnlyCollection<User>> GetAsync(CancellationToken token = default);
    
    Task<User> UpdateAsync(User user, CancellationToken token = default);
}