using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;

namespace TriaDemo.Service;

internal sealed class GroupService(CurrentUserService currentUserService, IGroupRepository groupRepository) : IGroupService
{
    public async Task<Group> CreateAsync(Group group, CancellationToken cancellationToken = default)
    {
        if (!await currentUserService.IsAdmin(cancellationToken))
        {
            throw new UnauthorizedException("User must be admin to create groups.");
        }
        
        var existingGroup = await groupRepository.GetByNameAsync(group.GroupName, cancellationToken);
        if (existingGroup is not null)
        {
            throw new NotUniqueException($"Group with name '{group.GroupName}' already exists.");
        }
        return await groupRepository.CreateAsync(group, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Group group, CancellationToken cancellationToken = default)
    {
        if (!await currentUserService.IsAdmin(cancellationToken))
        {
            throw new UnauthorizedException("User must be admin to delete groups.");
        }
        if (group.GroupName is Group.GroupAdmin or Group.GroupRegular)
        {
            throw new UnauthorizedException("Groups 'admin' and 'regular' are default and cannot be deleted.");
        }
        
        return await groupRepository.DeleteAsync(group.Id, cancellationToken);
    }

    public async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await groupRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Group>> GetAsync(CancellationToken cancellationToken = default)
    {
        return await groupRepository.GetAsync(cancellationToken);
    }

    public async Task<Group> UpdateAsync(Group group, CancellationToken cancellationToken = default)
    {
        if (!await currentUserService.IsAdmin(cancellationToken))
        {
            throw new UnauthorizedException("User must be admin to update groups.");
        }
        
        var existingGroup = await groupRepository.GetByNameAsync(group.GroupName, cancellationToken);
        
        if (existingGroup == null) return await groupRepository.UpdateAsync(group, cancellationToken);
        
        if (existingGroup.Id != group.Id)
        {
            throw new NotUniqueException($"Group with name '{group.GroupName}' already exists.");
        }

        return group;
    }
}