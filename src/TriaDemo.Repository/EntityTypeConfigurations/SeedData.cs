using TriaDemo.Service.Models;

namespace TriaDemo.Repository.EntityTypeConfigurations;

internal static class SeedData
{
    static SeedData()
    {
        var user = new User
        {
            Id = Guid.Parse("46e8d539-effb-4589-92c0-4d6d81c4c1d9"),
            FirstName = "Josh",
            LastName = "Doe",
            Email = "josh.doe@gmail.com",
            IsActive = true,
            PasswordHash = "AQAAAAIAAYagAAAAEL2J5/wSu5R9hegaDqliJkrwSunSP4iLaoA6ln28QUBKesrWFnjBiXV7Y8xVYiNVUQ==",
        };

        var groupAdmin = new Group
        {
            Id = Guid.Parse("ca280668-67c8-47be-b023-89d5b8a96366"),
            GroupName = "admin"
        };
        var groupReader = new Group
        {
            Id = Guid.Parse("e93d60bd-594d-48cc-a000-b14b252a4b17"),
            GroupName = "reader"
        };
        
        Users.Add(user);
        Groups.Add(groupAdmin);
        Groups.Add(groupReader);
        UserGroups.Add(new UserGroup { UserId = user.Id, GroupId = groupAdmin.Id });
    }

    public static List<User> Users { get; } = [];

    public static List<Group> Groups { get; } = [];
    
    public static List<UserGroup> UserGroups { get; } = [];
}