namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.People;
    using System;
    using System.Threading.Tasks;
    using DataModel.Models;
    using SocialNetwork.Web.Infrastructure;

    public class PeopleController : UserAreaController
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private const int SearchPeoplePageSize = 10;
        private const int FriendsPageSize = 10;

        public PeopleController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Search(string username, int page = 1)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var people = await _userService.ByContainingUsernamePaginationAsync(username, page, SearchPeoplePageSize);

            var viewModel = new SearchPeoplePaginationModel
            {
                Users = people,
                UsernameToSearch = username,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _userService.ByContainingUsernameCountAsync(username) / (double)SearchPeoplePageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Friends(string userId, int page = 1)
        {
            var loggedUserId = _userManager.GetUserId(User);

            if(!await this.CheckFriendshipStatus(loggedUserId, userId, _userService))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var friends = await _userService.FriendsPaginationAsync(userId, page, FriendsPageSize);

            var viewModel = new FriendsPagingModel
            {
                Friends = friends,
                UserId = userId,
                Names = await _userService.NamesByIdAsync(userId),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _userService.FriendsCountAsync(userId) / (double)FriendsPageSize)
            };

            return View(viewModel);
        }
    }
}
