namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Photos;
    using Web.Infrastructure;

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

        [HttpPost]
        public async Task<IActionResult> CreateAlbum(string title, string description)
        {
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description))
            {
                return BadRequest("Title and description must be provided");
            }

            var loggedUserId = _userManager.GetUserId(User);

            await _pictureService.CreateAlbumAsync(title, description, loggedUserId);

            return Ok();
        }

        public async Task<IActionResult> Album(int id)
        {
            var loggedUserId = _userManager.GetUserId(User);
            var albumOwnerId = await _pictureService.AlbumOwnerId(id);

            if (!await this.CheckFriendshipStatus(loggedUserId, albumOwnerId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var album = await _pictureService.AlbumByIdAsync(id);

            var viewModel = new AlbumModel
            {
                Album = album,
                MyAlbum = loggedUserId == albumOwnerId
            };

            return View(viewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var loggedUserId = _userManager.GetUserId(User);
            var albumOwnerId = await _pictureService.AlbumOwnerId(id);

            if (loggedUserId != albumOwnerId && !User.IsInRole(GlobalConstants.UserRole.Administrator))
            {
                return BadRequest("Error: Insufficient privileges");
            }

            var result = await _pictureService.DeleteAlbumByIdAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPictures(IFormCollection pictures)
        {
            var loggedUserId = _userManager.GetUserId(User);
            foreach (var pic in pictures.Files)
            {
                var (hasErrors, errors) = this.ValidateFile(pic);

                if (hasErrors)
                {
                    return BadRequest(string.Join("\n", errors));
                }

                // pic.Name is the album's id passed from ajax call
                await _pictureService.UploadPictureToAlbumAsync(int.Parse(pic.Name), loggedUserId, pic);
            }

            return Ok();
        }
    }
}