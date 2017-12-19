namespace SocialNetwork.Tests.Services
{
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using SocialNetwork.DataModel.Models;
    using SocialNetwork.Services;
    using SocialNetwork.Tests.Utils;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class PictureServiceTests
    {
        [Fact]
        public async Task AlbumOwnerIdShouldReturnNullIfAlbumDoesNotExist()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();

            var pictureService = new PictureService(db);

            // Act
            var result = await pictureService.AlbumOwnerId(1);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task AlbumOwnerIdShouldReturnTheIdOfUserIfAlbumExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var db = MockManager.GetMockDatabase();
            var user = new User
            {
                Id = userId
            };
            var album = new Album
            {
                Id = 1,
                Name = "My album",
                User = user
            };

            await db.Albums.AddAsync(album);
            await db.SaveChangesAsync();

            var pictureService = new PictureService(db);

            // Act
            var result = await pictureService.AlbumOwnerId(1);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .Be(userId);
        }

        [Fact]
        public async Task CreateAlbumShouldReturnTrueIfUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var db = MockManager.GetMockDatabase();
            var user = new User
            {
                Id = userId
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var pictureService = new PictureService(db);

            // Act
            var result = await pictureService.CreateAlbumAsync("New album", "New description",user.Id);

            // Assert
            result
                .Should()
                .Be(true);

            var userAlbum = await db.Albums.FirstOrDefaultAsync();

            userAlbum
                .Should()
                .NotBeNull();
        }
        
        [Fact]
        public async Task DeleteAlbumShouldReturnTrueIfAlbumExists()
        {
            // Arrange
            var db = MockManager.GetMockDatabase();
            var album = new Album
            {
                Id = 1,
                Pictures = new List<Picture>()
            };

            await db.Albums.AddAsync(album);
            await db.SaveChangesAsync();

            var pictureService = new PictureService(db);

            // Act
            var result = await pictureService.DeleteAlbumByIdAsync(1);

            // Assert
            result
                .Should()
                .Be(true);

            var albumExists = await db.Albums.FirstOrDefaultAsync();

            albumExists
                .Should()
                .BeNull();
        }
    }
}
