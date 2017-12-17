namespace SocialNetwork.Tests.Web.Controllers.Areas.User.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Controllers;
    using DataModel.Models;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;
    using System.Collections.Generic;
    using SocialNetwork.Web.Areas.User.Models.Profile;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Http;

    public class ProfileControllerTests
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
            result.As<ViewResult>()
                .Should()
                .NotBeNull();

            result.As<ViewResult>()
                .ViewName.ToLower()
                .Should()
                .Contain("notfound");
        }

        [Fact]
        public async Task VisitProfileShouldReturnViewWithModelForExistingUser()
        {
            // Arrange
            const string username = "Username";
            var userService = new Mock<IUserService>();
            var postService = new Mock<IPostService>();
            var signInManager = new Mock<SignInManager<User>>(
                null, null, null, null, null, null);

            userService
                .Setup(s => s.ByUsernameAsync(It.Is<string>(u => u == username)))
                .ReturnsAsync(new User { UserName = username });

            userService
                .Setup(s => s.ByUsernameAsync(It.Is<string>(u => u != username)))
                .ReturnsAsync(new User { UserName = "donkihot" });

            postService
                .Setup(p => p.ByUserIdAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Post> { new Post { Title = "Post Title" } });

            var user = GetMockLoggedUser();

            var controller = new ProfileController(userService.Object, null, postService.Object, null);

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

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

        private ClaimsPrincipal GetMockLoggedUser()
        {
            return new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.NameIdentifier, "1")
            }));
        }
    }
}
