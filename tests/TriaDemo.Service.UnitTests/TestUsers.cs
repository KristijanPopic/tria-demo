using TriaDemo.Service.Models;

namespace TriaDemo.Service.UnitTests;

public static class TestUsers
{
    public static User AdminUser = new()
    {
        Email = "admin@gmail.com",
        FirstName = "Admin",
        LastName = "Admin",
        IsActive = true,
        PasswordHash = "AQAAAAEAACcQAAAAEAAA==",
        Id = Guid.Parse("46e8d539-effb-4589-92c0-4d6d81c4c1d9"),
        Groups =
        [
            new Group
            {
                GroupName = Group.GroupAdmin
            }
        ]
    };

    public static User RegularUser1 = new()
    {
        Email = "josh.doe@gmail.com",
        FirstName = "Josh",
        LastName = "Doe",
        IsActive = true,
        PasswordHash = "AQAAAAEAACcQAAAAEAAA==",
        Id = Guid.Parse("ca280668-67c8-47be-b023-89d5b8a96366"),
        Groups =
        [
            new Group
            {
                GroupName = Group.GroupRegular
            }
        ]
    };
    
    public static User RegularUser2 = new()
    {
        Email = "miguel.hernandez@gmail.com",
        FirstName = "Miguel",
        LastName = "Hernandez",
        IsActive = true,
        PasswordHash = "AQAAAAEAACcQAAAAEAAA==",
        Id = Guid.Parse("efe19348-4aa2-46a1-a5c9-9da79a48b459"),
        Groups =
        [
            new Group
            {
                GroupName = Group.GroupRegular
            }
        ]
    };
}