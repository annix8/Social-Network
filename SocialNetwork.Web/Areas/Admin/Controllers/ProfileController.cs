namespace SocialNetwork.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Infrastructure;
    using System.Threading.Tasks;

    [Authorize(Roles = GlobalConstants.UserRole.Administrator)]
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> DeleteProfile(string usernameToDelete)
        {
            var result = await _userService.DeleteAccountAsync(usernameToDelete);

            if (!result)
            {
                return View(GlobalConstants.NotFoundView);
            }

            return Ok();
        }
    }
}