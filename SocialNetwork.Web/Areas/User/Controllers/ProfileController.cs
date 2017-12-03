
namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.DataModel.Enums;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Profile;
    using System.Linq;
    using System.Threading.Tasks;

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

            var viewModel = new VisitProfileModel
            {
                User = userToVisit,
               FriendshipStatus = await _userService.CheckFriendshipStatusAsync(userToVisit.Id, currentUser.Id)
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

        public async Task<IActionResult> CancelFriendRequest(string usernameToBefriend)
        {
            var result = await _userService.DeleteFriendshipAsync(User.Identity.Name, usernameToBefriend);

            return RedirectToAction(nameof(Visit), new { username = usernameToBefriend });
        }

        public async Task<IActionResult> PendingRequests(string userId)
        {

            return View();
        }
    }
}