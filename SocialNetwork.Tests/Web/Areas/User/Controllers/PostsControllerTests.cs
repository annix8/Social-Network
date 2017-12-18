namespace SocialNetwork.Tests.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using SocialNetwork.DataModel.Enums;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Tests.Extensions;
    using SocialNetwork.Tests.Utils;
    using SocialNetwork.Web.Areas.User.Controllers;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Xunit;

    public class PostsControllerTests
    {
        [Fact]
        public async Task DetailsShouldReturnNotFoundViewIfPostDoesNotExist()
        {
            // Arrange
            var postService = new Mock<IPostService>();

            var controller = new PostsController(postService.Object, null, null);

            // Act
            var result = await controller.Details(5);

            // Assert
            result.AssertNotFoundView();
        }

        [Fact]
        public async Task PostByUserNotInTheFriendListOfOtherNormalUserShouldReturnAccessDeniedView()
        {
            // Arrange
            var firstUserId = "FirstUserId";
            var secondUserId = "SecondUserId";
            var postService = new Mock<IPostService>();
            var userService = new Mock<IUserService>();
            var userManager = MockManager.GetMockUserManger();

            postService
                .Setup(s => s.ByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post { UserId = firstUserId });

            userManager
                .Setup(s => s.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(secondUserId);

            userService
                .Setup(s => s.CheckFriendshipStatusAsync(firstUserId, secondUserId))
                .ReturnsAsync((FriendshipStatus.NotFriend, ""));

            var controller = new PostsController(postService.Object, userService.Object, userManager.Object);
            controller.LoginMockUser();

            // Act
            var result = await controller.Details(5);

            // Assert
            result.AssertAccessDeniedView();
        }

        [Fact]
        public async Task EditShouldReturnNotFoundViewIfPostDoesNotExist()
        {
            // Arrange
            var postService = new Mock<IPostService>();

            var controller = new PostsController(postService.Object, null, null);

            // Act
            var result = await controller.Edit(5);

            // Assert
            result.AssertNotFoundView();
        }

        [Fact]
        public async Task EditForeignPostShouldReturnAccessDeniedView()
        {
            // Arrange
            var postService = new Mock<IPostService>();
            postService
                .Setup(s => s.ByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new Post {User = new User {UserName = "anotherUser" } });

            var controller = new PostsController(postService.Object, null, null);
            controller.LoginMockUser();

            // Act
            var result = await controller.Edit(5);

            // Assert
            result.AssertAccessDeniedView();
        }
    }
}
