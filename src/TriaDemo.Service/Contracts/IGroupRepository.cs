using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IGroupRepository
{
    Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Group>> GetGroupsAsync(IList<string> groupNames, CancellationToken cancellationToken = default);
}