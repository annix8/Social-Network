namespace SocialNetwork.Tests.Services
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SocialNetwork.DataModel.Models;
    using SocialNetwork.Services;
    using SocialNetwork.Tests.Utils;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public async Task ByUsernameAsyncShouldReturnOneUserWhenUserExists()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var secondUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testoviuser",
                Email = "tester@test.com",
                FirstName = "John",
                LastName = "Chloe"
            };

            await db.Users.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.ByUsernameAsync("testuser");

            // Assert
            result.UserName
                .Should()
                .NotBeNullOrEmpty()
                .And
                .Should()
                .Equals("testuser");
        }

        [Fact]
        public async Task MakeFriendRequestShouldMakeARecordInFriendshipTable()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var secondUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testoviuser",
                Email = "tester@test.com",
                FirstName = "John",
                LastName = "Chloe"
            };

            await db.Users.AddRangeAsync(firstUser, secondUser);
            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.MakeFriendRequestAsync(firstUser.UserName, secondUser.UserName);

            // Assert
            result
                .Should()
                .Be(true);

            var friendship = await db.Friendships.FirstOrDefaultAsync();

            friendship
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task MakeFriendRequestShouldReturnFalseIfOneOfUsersDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            await db.Users.AddAsync(firstUser);
            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.MakeFriendRequestAsync(firstUser.UserName, "madeUpUsername");

            // Assert
            result
                .Should()
                .Be(false);

            var friendship = await db.Friendships.FirstOrDefaultAsync();

            friendship
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task AcceptFriendshipShouldReturnFalseIfOneOfTheUsersDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            await db.Users.AddAsync(firstUser);
            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.AcceptFriendshipAsync(firstUser.UserName, "unexistingUser");

            // Assert
            result
                .Should()
                .Be(false);
        }

        [Fact]
        public async Task AcceptFriendshipShouldReturnTrueIfBothUsersExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var secondUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testoviuser",
                Email = "test@email.com",
                FirstName = "Blq",
                LastName = "Qlb"
            };

            await db.Users.AddRangeAsync(firstUser, secondUser);

            await db.SaveChangesAsync();

            var friendship = new Friendship
            {
                User = firstUser,
                Friend = secondUser,
                FriendshipIssuerId = firstUser.Id
            };

            await db.Friendships.AddAsync(friendship);
            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.AcceptFriendshipAsync(firstUser.UserName, secondUser.UserName);

            // Assert
            result
                .Should()
                .Be(true);
        }

        [Fact]
        public async Task DeleteAccountShouldReturnTrueIfUserExists()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var firstUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testuser",
                Email = "test@test.com",
                FirstName = "John",
                LastName = "Doe"
            };

            var secondUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "testoviuser",
                Email = "test@email.com",
                FirstName = "Blq",
                LastName = "Qlb"
            };

            await db.Users.AddRangeAsync(firstUser, secondUser);

            await db.SaveChangesAsync();

            var userService = new UserService(db);

            // Act
            var result = await userService.DeleteAccountAsync(firstUser.UserName);

            // Assert
            result
                .Should()
                .Be(true);

            var users = await db.Users.ToListAsync();

            users
                .Should()
                .HaveCount(1, "Because one of the two users in the db was deleted");
        }

        [Fact]
        public async Task DeleteAccountShouldReturnFalseIfUserDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var userService = new UserService(db);

            // Act
            var result = await userService.DeleteAccountAsync("unexistingUser");

            // Assert
            result
                .Should()
                .Be(false);
        }
    }
}
