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

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken token = default)
    {
        return await dbContext.Users.Where(u => u.Id == userId).ExecuteDeleteAsync(token) > 0;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token = default)
    {
        return await dbContext.Users.SingleOrDefaultAsync(u => u.Email == email, token);
    }

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default)
    {
        return await dbContext.Users.Include(u => u.Groups).SingleOrDefaultAsync(u => u.Id == id, token);
    }
}