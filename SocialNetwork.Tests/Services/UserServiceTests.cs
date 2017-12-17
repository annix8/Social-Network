namespace SocialNetwork.Tests.Services
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SocialNetwork.DataModel;
    using SocialNetwork.DataModel.Models;
    using SocialNetwork.Services;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class UserServiceTests
    {
        [Fact]
        public async Task ByUsernameAsyncShouldReturnOneUserWhenUserExists()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<SocialNetworkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new SocialNetworkDbContext(dbOptions);

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
    }
}
