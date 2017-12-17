namespace SocialNetwork.Tests.Web.Controllers.Areas.User.Controllers
{
    using DataModel.Models;
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Tests.Extensions;
    using SocialNetwork.Web.Areas.User.Controllers;
    using SocialNetwork.Web.Areas.User.Models.Profile;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class UserProfileControllerTests
    {
        [Fact]
        public void ProfileControllerShouldBeForAuthorizedUsersOnly()
        {
            var profileControllerAttributes = typeof(ProfileController).GetCustomAttributes(true);

            profileControllerAttributes
                .Should()
                .Match(a => a.Any(attr => attr.GetType() == typeof(AuthorizeAttribute)));
        }

        [Fact]
        public async Task VisitProfileShouldReturnNotFoundViewForUnexistingUser()
        {
            // Arrange
            var userService = new Mock<IUserService>();

            var controller = new ProfileController(userService.Object, null, null, null);

            // Act
            var result = await controller.Visit("unexisting");

            // Assert
            result.AssertNotFoundView();
        }

        [Fact]
        public async Task VisitProfileShouldReturnViewWithModelForExistingUser()
        {
            // Arrange
            const string username = "Username";
            var userService = new Mock<IUserService>();
            var postService = new Mock<IPostService>();

            userService
                .Setup(s => s.ByUsernameAsync(It.Is<string>(u => u == username)))
                .ReturnsAsync(new User { UserName = username });

            userService
                .Setup(s => s.ByUsernameAsync(It.Is<string>(u => u != username)))
                .ReturnsAsync(new User { UserName = "donkihot" });

            postService
                .Setup(p => p.ByUserIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Post> { new Post { Title = "Post Title" } });

            var controller = new ProfileController(userService.Object, null, postService.Object, null);
            controller.LoginMockUser();

            // Act
            var result = await controller.Visit(username);

            // Assert
            result
                .Should()
                .BeOfType<ViewResult>()
                .Subject
                .Model
                .Should()
                .BeOfType<VisitProfileModel>();
        }

        [Fact]
        public async Task VisitUsernameThatHasSameNameAsLoggedUserShouldReturnRedirectToActionResult()
        {
            // Arrange
            const string username = "Username";
            var userService = new Mock<IUserService>();

            userService
                .Setup(s => s.ByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User { UserName = username });

            var controller = new ProfileController(userService.Object, null, null, null);
            controller.LoginMockUser();

            // Act
            var result = await controller.Visit(username);

            // Assert
            result
                .Should()
                .BeOfType<RedirectToActionResult>()
                .Subject
                .Should()
                .Match(s => s.As<RedirectToActionResult>().ActionName == nameof(ProfileController.MyProfile));      
        }
    }
}
