using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityFrameworkCore;

internal sealed class GroupRepository(TriaDemoDbContext dbContext) : IGroupRepository
{
    private readonly DbSet<Group> _dbSet = dbContext.Groups;

    public async Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default)
    {
        _dbSet.Add(group);
        await dbContext.SaveChangesAsync(cancellationToken);
        return group;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(u => u.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleAsync(g => g.GroupName == "reader", cancellationToken);
    }

    public async Task<IReadOnlyCollection<Group>> GetAsync(IList<string> groupNames, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(g => groupNames.Contains(g.GroupName)).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Group>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);

    }

    public async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<Group?> GetByNameAsync(string groupName, CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleOrDefaultAsync(u => u.GroupName == groupName, cancellationToken);
    }

    public async Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        var entry = _dbSet.Entry(group);
        if (entry.State != EntityState.Modified)
        {
            _dbSet.Update(group);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return group;
    }
}