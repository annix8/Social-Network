namespace SocialNetwork.Web.Areas.Admin.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;
    using Web.Infrastructure;

    [Authorize(Roles = GlobalConstants.UserRole.Administrator)]
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public ProfileController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
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

        [HttpPost]
        public async Task<IActionResult> MakeAdmin(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest();
            }

            await _userManager.AddToRoleAsync(user, GlobalConstants.UserRole.Administrator);
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DemoteToUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest();
            }

            await _userManager.RemoveFromRoleAsync(user, GlobalConstants.UserRole.Administrator);

            return Ok();
        }
    }
}