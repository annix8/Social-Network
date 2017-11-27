using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using SocialNetwork.Services.Contracts;
    using System.Threading.Tasks;

    [Authorize]
    public class ProfileController : UserAreaController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userService.ByUsername(User.Identity.Name);
            return View(user);
        }
    }
}