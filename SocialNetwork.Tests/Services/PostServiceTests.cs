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

    public class PostServiceTests
    {
        [Fact]
        public async Task CreateShouldReturnTrueIfUserExists()
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

            var postService = new PostService(db);

            // Act
            var result = await postService.CreateAsync(firstUser.UserName, "Some title", "Some Content");

            // Assert
            result
                .Should()
                .Be(true);

            var post = await db.Posts.FirstOrDefaultAsync();

            post
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task CreateShouldReturnFalseIfUserDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var postService = new PostService(db);

            // Act
            var result = await postService.CreateAsync("unexistingUsername", "Some title", "Some Content");

            // Assert
            result
                .Should()
                .Be(false);

            var post = await db.Posts.FirstOrDefaultAsync();

            post
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task DeletePostShouldReturnTrueIfPostExists()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var post = new Post
            {
                Id = 1,
                Title = "Title",
                Content = "Content"
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postService = new PostService(db);

            // Act
            var result = await postService.DeleteAsync(1);

            // Assert
            result
                .Should()
                .Be(true);

            var hasPost = await db.Posts.FirstOrDefaultAsync();

            hasPost
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task EditShouldSuccessfullyChangeTitleOfExistingPost()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var post = new Post
            {
                Id = 1,
                Title = "Title",
                Content = "Content"
            };

            await db.Posts.AddAsync(post);
            await db.SaveChangesAsync();

            var postService = new PostService(db);

            // Act
            var result = await postService.EditAsync(1,"New title", "New content");

            // Assert
            result
                .Should()
                .Be(true);

            var hasPost = await db.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);

            hasPost
                .Should()
                .NotBeNull()
                .And
                .Match(m => m.As<Post>().Title == "New title");
        }
    }
}
