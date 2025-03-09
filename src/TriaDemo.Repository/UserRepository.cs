using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository;

internal sealed class UserRepository(TriaDemoDbContext dbContext) : IUserRepository
{
    public async Task<User> CreateAsync(User user, CancellationToken token = default)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(token);
        return user;
    }

    public async Task<bool> DeleteAsync(User user, CancellationToken token = default)
    {
        return await dbContext.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync(token) > 0;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email, token);
    }
}