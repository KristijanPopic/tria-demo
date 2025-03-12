using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore;

internal sealed class UserRepository(TriaDemoDbContext dbContext) : IUserRepository
{
    private readonly DbSet<User> _dbSet = dbContext.Users;

    public async Task<User> CreateAsync(User user, CancellationToken token = default)
    {
        _dbSet.Add(user);
        await dbContext.SaveChangesAsync(token);
        return user;
    }

    public async Task<bool> DeleteAsync(Guid userId, CancellationToken token = default)
    {
        return await _dbSet.Where(u => u.Id == userId).ExecuteDeleteAsync(token) > 0;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token = default)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Email == email, token);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await _dbSet
            .Include(u => u.Groups)
            .SingleOrDefaultAsync(u => u.Id == id, token);
    }

    public async Task<IDictionary<Guid, User>> GetByIdAsync(IEnumerable<Guid> ids, CancellationToken token = default)
    {
        return await _dbSet
            .Include(u => u.Groups)
            .Where(u => ids.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u, token);
    }

    public async Task<IReadOnlyCollection<User>> GetAsync(CancellationToken token = default)
    {
        return await _dbSet
            .Include(u => u.Groups)
            .ToListAsync(token);
    }

    public async Task<User> UpdateAsync(User user, CancellationToken token = default)
    {
        var entry = _dbSet.Entry(user);
        if (entry.State != EntityState.Modified)
        {
            _dbSet.Update(user);
        }

        await dbContext.SaveChangesAsync(token);
        return user;
    }
}