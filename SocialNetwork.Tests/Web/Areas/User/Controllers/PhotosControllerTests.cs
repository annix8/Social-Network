namespace SocialNetwork.Tests.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Controllers;
    using DataModel.Models;
    using Xunit;
    using System.Security.Claims;
    using System;
    using System.Threading.Tasks;
    using SocialNetwork.Tests.Extensions;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Tests.Utils;

    public class PhotosControllerTests
    {
        [Fact]
        public async Task CreateAlbumShouldReturnOkIfTitleAndDescriptionAreProvided()
        {
            // Arrange
            var pictureService = new Mock<IPictureService>();
            var userManager = MockManager.GetMockUserManger();

            pictureService
                .Setup(s => s.CreateAlbumAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            userManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(Guid.NewGuid().ToString());

            var controller = new PhotosController(pictureService.Object, null, userManager.Object);
            controller.LoginMockUser();

            // Act
            var result = await controller.CreateAlbum("fakeTitle", "fakeDescription");

            // Assert
            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Fact]
        public async Task CreateAlbumShouldReturnBadRequestIfTitleIsNotProvided()
        {
            // Arrange
            var pictureService = new Mock<IPictureService>();

            pictureService
                .Setup(s => s.CreateAlbumAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new PhotosController(pictureService.Object, null, null);

            // Act
            var result = await controller.CreateAlbum(null, "fakeDescription");

            // Assert
            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateAlbumShouldReturnBadRequestIfDescriptionIsNotProvided()
        {
            // Arrange
            var pictureService = new Mock<IPictureService>();

            pictureService
                .Setup(s => s.CreateAlbumAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new PhotosController(pictureService.Object, null, null);

            // Act
            var result = await controller.CreateAlbum("fakeTitle", null);

            // Assert
            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task DeleteAlbumShouldReturnOkIfTheUserDeletesOwnPicture()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var pictureService = new Mock<IPictureService>();
            var userManager = MockManager.GetMockUserManger();

            pictureService
                .Setup(s => s.AlbumOwnerId(It.IsAny<int>())).
                ReturnsAsync(userId);
            pictureService
                .Setup(s => s.DeleteAlbumByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(true);

            userManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            var controller = new PhotosController(pictureService.Object, null, userManager.Object);
            controller.LoginMockUser();

            // Act
            var result = await controller.DeleteAlbum(1);

            // Assert
            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteAlbumShouldReturnBadRequestIfUserTriesToDeleteForeignAlbum()
        {
            // Arrange
            var pictureService = new Mock<IPictureService>();
            var userManager = MockManager.GetMockUserManger();

            pictureService
                .Setup(s => s.AlbumOwnerId(It.IsAny<int>())).
                ReturnsAsync(Guid.NewGuid().ToString());

            userManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(Guid.NewGuid().ToString());

            var controller = new PhotosController(pictureService.Object, null, userManager.Object);
            controller.LoginMockUser();

            // Act
            var result = await controller.DeleteAlbum(1);

            // Assert
            result
                .Should()
                .BeOfType<BadRequestObjectResult>();
        }
    }
}
