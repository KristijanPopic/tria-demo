using TriaDemo.Service.Models;

namespace TriaDemo.Service;

public interface IGroupService
{
    Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Group group, CancellationToken cancellationToken = default);
    
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Group>> GetAsync(CancellationToken cancellationToken = default);
    
    Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default);
}