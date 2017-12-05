﻿
namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Enums;
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Profile;
    using Web.Infrastructure;

    public class ProfileController : UserAreaController
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly UserManager<User> _userManager;

        public ProfileController(IUserService userService, IPictureService pictureService, UserManager<User> userManager)
        {
            _userService = userService;
            _pictureService = pictureService;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userService.ByUsernameAsync(User.Identity.Name);
            user.Posts = user.Posts.OrderByDescending(p => p.PublishedOn).ToList();

            var viewModel = new MyProfileModel
            {
                User = user,
                PendingRequestsCount = user.FriendRequestsAccepted.Where(x => x.FriendshipStatus == FriendshipStatus.Pending).Count()
            };

            return View(viewModel);

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

            if (userToVisit.UserName == currentUser.UserName)
            {
                return RedirectToAction(nameof(MyProfile));
            }

            var (friendshipStatus, issuerName) = await _userService.CheckFriendshipStatusAsync(userToVisit.Id, currentUser.Id);

            var viewModel = new VisitProfileModel
            {
                User = userToVisit,
                FriendshipStatus = friendshipStatus,
                IssuerUsername = issuerName
            };

            return View(viewModel);
        }

        public async Task<IActionResult> SendFriendRequest(string usernameToBefriend)
        {
            var friendshipResult = await _userService.MakeFriendRequestAsync(User.Identity.Name, usernameToBefriend);

            if (!friendshipResult)
            {
                //TODO: log errors
                return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
            }

            return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
        }

        public async Task<IActionResult> CancelFriendRequest(string usernameToBefriend, string returnUrl = null)
        {
            var result = await _userService.DeleteFriendshipAsync(User.Identity.Name, usernameToBefriend);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
        }

        public async Task<IActionResult> AcceptFriendRequest(string usernameToBefriend, string returnUrl = null)
        {
            var loggedUserId = _userManager.GetUserId(User);
            var userToBefriendId = await _userService.IdByUsernameAsync(usernameToBefriend);

            if(!await _userService.ValidateFriendshipAcceptance(loggedUserId, userToBefriendId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var result = await _userService.AcceptFriendshipAsync(User.Identity.Name, usernameToBefriend);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
        }

        public async Task<IActionResult> PendingRequests(string userId)
        {
            var users = await _userService.PendingFriendsAsync(userId);

            var viewModel = users
                .Select(u => new PendingRequestsModel
                {
                    UserId = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Username = u.UserName
                });

            return View(viewModel);
        }
    }
}