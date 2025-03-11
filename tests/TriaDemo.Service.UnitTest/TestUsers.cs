using TriaDemo.Service.Models;

namespace TriaDemo.Service.UnitTest;

public static class TestUsers
{
    public static User AdminUser = new User
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
                GroupName = "admin"
            }
        ]
    };

    public static User ReaderUser1 = new User
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
                GroupName = "reader"
            }
        ]
    };
    
    public static User ReaderUser2 = new User
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
                GroupName = "reader"
            }
        ]
    };
}