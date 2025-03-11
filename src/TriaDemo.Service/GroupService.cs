using TriaDemo.Service.Contracts;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public sealed class GroupService(IGroupRepository groupRepository) : IGroupService
{
    public Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Group group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<Group>> GetAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}