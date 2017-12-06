namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Posts;

    public class HomeController : UserAreaController
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;

        public HomeController(IUserService userService, IPostService postService)
        {
            _userService = userService;
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var loggedUser = await _userService.ByUsernameAsync(User.Identity.Name);
            var lastTenFriendPosts = await _postService.FriendsPostsAsync(loggedUser.Id);

            var viewModel = new HomepagePostModel
            {
                Posts = lastTenFriendPosts
            };

            return View(viewModel);
        }
    }
}