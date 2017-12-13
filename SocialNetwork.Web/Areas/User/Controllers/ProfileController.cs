namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Enums;
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Profile;
    using Web.Infrastructure;

    public class ProfileController : UserAreaController
    {
        private readonly IUserService _userService;
        private readonly IPictureService _pictureService;
        private readonly IPostService _postService;
        private readonly UserManager<User> _userManager;
        private const int PostPageSize = 5;

        public ProfileController(IUserService userService,
              IPictureService pictureService,
              IPostService postService,
              UserManager<User> userManager)
        {
            _userService = userService;
            _pictureService = pictureService;
            _postService = postService;
            _userManager = userManager;
        }

        public async Task<IActionResult> MyProfile(int page = 1)
        {
            var user = await _userService.ByUsernameAsync(User.Identity.Name);
            var usersPosts = await _postService.ByUserIdAsync(user.Id, page, PostPageSize);

            var viewModel = new MyProfileModel
            {
                User = user,
                Posts = usersPosts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _postService.ByUserIdCountAsync(user.Id) / (double)PostPageSize),
                PendingRequestsCount = user.FriendRequestsAccepted.Where(x => x.FriendshipStatus == FriendshipStatus.Pending).Count()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            var (hasErrors,errors) = this.ValidateFile(file);

            if (hasErrors)
            {
                return BadRequest(string.Join("\n",errors));
            }

            await _pictureService.UploadProfilePictureAsync(User.Identity.Name, file);
            
            return Ok();
        }

        public async Task<IActionResult> Visit(string username, int page = 1)
        {
            var userToVisit = await _userService.ByUsernameAsync(username);
            if(userToVisit == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            var currentUser = await _userService.ByUsernameAsync(User.Identity.Name);

            if (userToVisit.UserName == currentUser.UserName)
            {
                return RedirectToAction(nameof(MyProfile));
            }

            var (friendshipStatus, issuerName) = await _userService.CheckFriendshipStatusAsync(userToVisit.Id, currentUser.Id);
            var userToVisitPosts = await _postService.ByUserIdAsync(userToVisit.Id, page, PostPageSize);

            var viewModel = new VisitProfileModel
            {
                User = userToVisit,
                Posts = userToVisitPosts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _postService.ByUserIdCountAsync(userToVisit.Id) / (double)PostPageSize),
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
                return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
            }

            return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
        }

        public async Task<IActionResult> CancelFriendRequest(string usernameToUnfriend, string returnUrl = null)
        {
            var result = await _userService.DeleteFriendshipAsync(User.Identity.Name, usernameToUnfriend);
            if (!result)
            {
                return RedirectToAction(nameof(Visit), new { username = usernameToUnfriend });
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(Visit), new { username = usernameToUnfriend });
        }

        public async Task<IActionResult> AcceptFriendRequest(string usernameToBefriend, string returnUrl = null)
        {
            var loggedUserId = _userManager.GetUserId(User);
            var userToBefriendId = await _userService.IdByUsernameAsync(usernameToBefriend);

            if (!await _userService.ValidateFriendshipAcceptance(loggedUserId, userToBefriendId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var result = await _userService.AcceptFriendshipAsync(User.Identity.Name, usernameToBefriend);

            if (!result)
            {
                return View(GlobalConstants.AccessDeniedView);
            }

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