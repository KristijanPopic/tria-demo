using Microsoft.EntityFrameworkCore;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Repository;

internal sealed class GroupRepository(TriaDemoDbContext dbContext) : IGroupRepository
{
    public Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.Groups.SingleAsync(g => g.GroupName == "reader", cancellationToken);
    }
}