using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Contracts;
using SocialNetwork.Web.Areas.User.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    public class PeopleController : UserAreaController
    {
        private readonly IUserService _userService;
        private const int PageSize = 10;

        public PeopleController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Search(string username, int page = 1)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var people = await _userService.ByContainingUsernamePaginationAsync(username, page, PageSize);

            var viewModel = new SearchPeoplePaginationModel
            {
                Users = people,
                UsernameToSearch = username,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(await _userService.ByContainingUsernameCountAsync(username) / (double)PageSize)
            };

            return View(viewModel);
        }
    }
}
