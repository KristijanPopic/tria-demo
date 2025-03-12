using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IUserRepository
{
    Task<User> CreateAsync(User user, CancellationToken token = default);
    
    Task<bool> DeleteAsync(Guid userId, CancellationToken token = default);
    
    Task<User?> GetByEmailAsync(string email, CancellationToken token = default);
    
    Task<User?> GetByIdAsync(Guid id, CancellationToken token = default);
    
    Task<IDictionary<Guid, User>> GetByIdAsync(IEnumerable<Guid> ids, CancellationToken token = default);
    
    Task<IReadOnlyCollection<User>> GetAsync(CancellationToken token = default);
    
    Task<User> UpdateAsync(User user, CancellationToken token = default);
}