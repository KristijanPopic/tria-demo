using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TriaDemo.RestApi.Authorization;
using TriaDemo.Service;
using TriaDemo.Service.Models;

namespace TriaDemo.RestApi.Controllers.Groups;

[Route("api/groups")]
[Authorize]
public class GroupController(IGroupService groupService) : ApiControllerBase
{
    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AuthorizeAdmin]
    public async Task<ActionResult<GroupResponse>> CreateGroup(CreateGroupRequest request, [FromServices]IValidator<CreateGroupRequest> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var group = new Group { Id = Guid.NewGuid(),GroupName = request.GroupName };
        
        var createdGroup = await groupService.CreateAsync(group, cancellationToken);
        return Ok(GroupResponse.From(createdGroup));
    }
    
    /// <summary>
    /// Deletes the group. Requires administrator rights.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAdmin]
    public async Task<ActionResult> DeleteGroup(Guid id, CancellationToken cancellationToken = default)
    {
        var group = await groupService.GetByIdAsync(id, cancellationToken);
        if (group is null)
        {
            return GroupNotFoundProblem(id);
        }
        
        await groupService.DeleteAsync(group, cancellationToken);
        return NoContent();
    }
    
    /// <summary>
    /// Gets the list of groups.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<IEnumerable<GroupResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<GroupResponse>>> GetGroups(CancellationToken cancellationToken = default)
    {
        // this would have paging in real-world application
        var groups = await groupService.GetAsync(cancellationToken);
        return Ok(groups.Select(GroupResponse.From));
    }
    
    /// <summary>
    /// Gets a single group by its identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupResponse>> GetGroup(Guid id, CancellationToken cancellationToken = default)
    {
        var group = await groupService.GetByIdAsync(id, cancellationToken);
        if (group is null)
        {
            return GroupNotFoundProblem(id);
        }
        return Ok(GroupResponse.From(group));
    }
    
    /// <summary>
    /// Updates the group. Requires administrator rights.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="validator"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType<GroupResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AuthorizeAdmin]
    public async Task<ActionResult<GroupResponse>> UpdateGroup(UpdateGroupRequest request, IValidator<UpdateGroupRequest> validator, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ValidationProblem(validationResult);
        }
        
        var group = await groupService.GetByIdAsync(request.Id, cancellationToken);
        if (group is null)
        {
            return GroupNotFoundProblem(request.Id);        
        }
        
        group.GroupName = request.GroupName;
        
        var updatedUser = await groupService.UpdateAsync(group, cancellationToken);
        
        return Ok(GroupResponse.From(updatedUser));
    }

    private ObjectResult GroupNotFoundProblem(Guid groupId)
    {
        return NotFoundProblem(title: "Group not found", detail: $"Group with id {groupId} not found.");
    }
}