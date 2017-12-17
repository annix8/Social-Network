namespace SocialNetwork.Tests.Web.Areas.Admin.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Tests.Extensions;
    using SocialNetwork.Web.Areas.Admin.Controllers;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class AdminProfileControllerTests
    {
        [Fact]
        public void ProfileControllerShouldBeForAdministratorsOnly()
        {
            var profileControllerAttributes = typeof(ProfileController).GetCustomAttributes(true);

            var authorizeAttribute = profileControllerAttributes
                .FirstOrDefault(a => a.GetType() == typeof(AuthorizeAttribute));

            authorizeAttribute
                .Should()
                .NotBeNull();

            var rolesPropertyValue = authorizeAttribute
                 .GetType()
                 .GetProperties()
                 .FirstOrDefault(p => p.Name == "Roles")
                 .GetValue((AuthorizeAttribute)authorizeAttribute).ToString();

            rolesPropertyValue
                .Should()
                .Contain("Administrator");
        }

        [Fact]
        public async Task DeleteProfileShouldReturnOkIfUserExists()
        {
            // Arrange
            var username = "Username";

            var userService = new Mock<IUserService>();
            userService
                .Setup(s => s.DeleteAccountAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new ProfileController(userService.Object, null);

            // Act
            var result = await controller.DeleteProfile(username);

            // Assert
            result
                .Should()
                .BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteProfileShouldReturnNotFoundViewIfUserDoesNotExist()
        {
            // Arrange
            var username = "Username";

            var userService = new Mock<IUserService>();
            userService
                .Setup(s => s.DeleteAccountAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new ProfileController(userService.Object, null);

            // Act
            var result = await controller.DeleteProfile(username);

            // Assert
            result.AssertNotFoundView();
        }
    }
}
