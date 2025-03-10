using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository;

internal sealed class GroupRepository(TriaDemoDbContext dbContext) : IGroupRepository
{
    private readonly DbSet<Group> _dbSet = dbContext.Groups;
    
    public async Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.SingleAsync(g => g.GroupName == "reader", cancellationToken);
    }

    public async Task<IReadOnlyCollection<Group>> GetGroupsAsync(IList<string> groupNames, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(g => groupNames.Contains(g.GroupName)).ToListAsync(cancellationToken);
    }
}