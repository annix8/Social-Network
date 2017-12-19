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

    public class MessageServiceTests
    {
        [Fact]
        public async Task CreateMessageShouldReturnTrueIfBothSenderAndReceiverExist()
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

            var messageService = new MessageService(db);

            // Act
            var result = await messageService.CreateMessageAsync(firstUser.UserName,secondUser.UserName, "Some content");

            // Assert
            result
                .Should()
                .Be(true);

            var messageExists = await db.Messages.FirstOrDefaultAsync();

            messageExists
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task CreateMessageShouldReturnFalseIfOneOfTheUsersDoesNotExist()
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

            var messageService = new MessageService(db);

            // Act
            var result = await messageService.CreateMessageAsync(firstUser.UserName, "unexisting", "Some content");

            // Assert
            result
                .Should()
                .Be(false);

            var messageExists = await db.Messages.FirstOrDefaultAsync();

            messageExists
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task DeleteMessageShouldReturnTrueIfMessageExists()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var message = new Message
            {
                Id = 1
            };

            await db.Messages.AddAsync(message);
            await db.SaveChangesAsync();

            var messageService = new MessageService(db);

            // Act
            var result = await messageService.DeleteMessageAsync(1);

            // Assert
            result
                .Should()
                .Be(true);

            var messageExists = await db.Messages.FirstOrDefaultAsync();

            messageExists
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task DeleteMessageShouldReturnFalseIfMessageDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var messageService = new MessageService(db);

            // Act
            var result = await messageService.DeleteMessageAsync(1);

            // Assert
            result
                .Should()
                .Be(false);
        }
    }
}
