using TriaDemo.Service.Models;

namespace TriaDemo.Service.Contracts;

public interface IGroupRepository
{
    Task<Group> GetReaderGroupAsync(CancellationToken cancellationToken = default);
}