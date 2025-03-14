﻿using Moq;
using TriaDemo.Service.Contracts;
using TriaDemo.Service.Exceptions;
using TriaDemo.Service.Models;
using TriaDemo.Service.UnitTests.TestDoubles;

namespace TriaDemo.Service.UnitTests;

public sealed class UserServiceTests
{
    [Fact]
    public async Task Admin_user_can_successfully_delete_other_users()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        var userToDelete = TestUsers.RegularUser2;
        var sut = CreateSut(userRepositoryMock, authenticatedUser: TestUsers.AdminUser);

        var result = await sut.DeleteAsync(userToDelete, CancellationToken.None);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task Non_admin_user_can_not_delete_users()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(m => m.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        var userToDelete = TestUsers.RegularUser2;
        var sut = CreateSut(userRepositoryMock, authenticatedUser: TestUsers.RegularUser1);

        await Assert.ThrowsAsync<UnauthorizedException>(async () => await sut.DeleteAsync(userToDelete, CancellationToken.None));
    }

    [Fact]
    public async Task New_user_is_created_in_regular_group()
    {
        var user = new User
        {
            Email = "josh.doe@gmail.com",
            FirstName = "Josh",
            LastName = "Doe",
            PasswordHash = "AQAAAAEAACcQAAAAEAAA==",
            Id = Guid.NewGuid()
        };
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(m => m.CreateAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        var sut = CreateSut(userRepositoryMock);
        
        var result = await sut.CreateAsync(user, CancellationToken.None);

        Assert.Contains(result.Groups, g => g.GroupName == Group.GroupRegular);
    }

    private static UserService CreateSut(Mock<IUserRepository> userRepositoryMock, User? authenticatedUser = null)
    {
        var currentUser = new CurrentUserStub
        {
            Email = authenticatedUser?.Email ?? "jane.doe@example.com",
            IsAuthenticated = true,
            UserId = authenticatedUser?.Id ?? Guid.Parse("46e8d539-effb-4589-92c0-4d6d81c4c1d9")
        };
        
        userRepositoryMock
            .Setup(m => m.GetByIdAsync(currentUser.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => authenticatedUser);

        var currentUserService = new CurrentUserService(currentUser, userRepositoryMock.Object);
        
        var groupRepositoryMock = new Mock<IGroupRepository>();
        groupRepositoryMock
            .Setup(m => m.GetRegularGroupAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Group { Id = Guid.Parse("e93d60bd-594d-48cc-a000-b14b252a4b17"), GroupName = Group.GroupRegular });

        return new UserService(currentUserService, userRepositoryMock.Object, groupRepositoryMock.Object);
        
    }
}