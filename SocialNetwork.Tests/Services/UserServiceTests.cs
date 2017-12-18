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
    }
}
