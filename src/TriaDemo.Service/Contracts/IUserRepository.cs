using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IUserRepository
{
    public Task<User> CreateAsync(User user, CancellationToken token = default);
    
    public Task<bool> DeleteAsync(User user, CancellationToken token = default);
}