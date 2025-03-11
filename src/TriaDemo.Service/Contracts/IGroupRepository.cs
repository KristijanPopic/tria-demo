using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IGroupRepository
{
    Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Group>> GetAsync(IList<string> groupNames, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Group>> GetAsync(CancellationToken cancellationToken = default);
    
    Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<Group?> GetByNameAsync(string groupName, CancellationToken cancellationToken = default);
    
    Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default);
}