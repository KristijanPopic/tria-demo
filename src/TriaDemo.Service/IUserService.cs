using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface IUserService
{
    Task<User> CreateUserAsync(User user, CancellationToken token = default);
}