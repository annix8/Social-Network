using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using SocialNetwork.DataModel.Enums;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Infrastructure;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
    public class ProfileController : UserAreaController
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;

        public ProfileController(IUserService userService, IPictureService pictureService)
        {
            _userService = userService;
            _pictureService = pictureService;
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userService.ByUsernameAsync(User.Identity.Name);
            user.Posts = user.Posts.OrderByDescending(p => p.PublishedOn).ToList();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            // TODO: check file extension and log errors
            if (file == null || file.Length < 0)
            {
                return BadRequest();
            }
            else
            {
                await _pictureService.UploadProfilePictureAsync(User.Identity.Name, file);
            }

            return RedirectToAction(nameof(MyProfile));
        }

        public async Task<IActionResult> Visit(string username)
        {
            var userToVisit = await _userService.ByUsernameAsync(username);
            var currentUser = await _userService.ByUsernameAsync(User.Identity.Name);

            var friendshipStatus = await _userService.CheckFriendshipStatus(userToVisit.Id, currentUser.Id);

            switch (friendshipStatus)
            {
                case FriendshipStatus.Blocked:
                case FriendshipStatus.NotFriend:
                case FriendshipStatus.Pending:
                    {
                        userToVisit.IsPublic = false;
                        break;
                    }
                case FriendshipStatus.Accepted:
                    {
                        userToVisit.IsPublic = true;
                        break;
                    }
                default:break;
            }

            return View(userToVisit);
        }
    }
}