namespace SocialNetwork.Tests.Web.Controllers.Areas.User.Controllers
{
    using FluentAssertions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Controllers;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

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
            var userService = new Mock<IUserService>();

            var controller = new ProfileController(userService.Object, null, null, null);

            var result = await controller.Visit("unexisting");

            result.As<ViewResult>()
                .Should()
                .NotBeNull();

            result.As<ViewResult>()
                .ViewName.ToLower()
                .Should()
                .Contain("notfound"); 
        }
    }
}
