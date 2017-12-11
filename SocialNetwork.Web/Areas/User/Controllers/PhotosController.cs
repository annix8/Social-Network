namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Photos;
    using SocialNetwork.Web.Infrastructure;
    using System.Threading.Tasks;

    public class PhotosController : UserAreaController
    {
        private readonly IPictureService _pictureService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public PhotosController(IPictureService pictureService,
            IUserService userService,
            UserManager<User> userManager)
        {
            _pictureService = pictureService;
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Albums(string userId)
        {
            var loggedUserId = _userManager.GetUserId(User);

            if (!await this.CheckFriendshipStatus(loggedUserId, userId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var albums = await _pictureService.UserAlbumsAsync(userId);

            var viewModel = new AlbumsModel
            {
                Albums = albums,
                AlbumsOwnerNames = await _userService.NamesByIdAsync(userId),
                MyAlbums = loggedUserId == userId
            };

            return View(viewModel);
        }
    }
}