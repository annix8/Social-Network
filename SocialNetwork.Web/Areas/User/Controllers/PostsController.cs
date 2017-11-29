using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Services.Contracts;
using SocialNetwork.Web.Areas.User.Models;
using SocialNetwork.Web.Areas.User.Models.Posts;
using System.Threading.Tasks;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    [Authorize]
    public class PostsController : UserAreaController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var postResult = postModel.Picture == null ?
               await _postService.CreateAsync(User.Identity.Name, postModel.Title, postModel.Content) :
               await _postService.CreateAsync(User.Identity.Name, postModel.Title, postModel.Content, postModel.Picture);

            return RedirectToAction(nameof(ProfileController.MyProfile), "Profile");
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.ByIdAsync(id);

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _postService.MakeCommentAsync(commentModel.Comment, commentModel.PostId, commentModel.CommentAuthor);

            return RedirectToAction(nameof(Details), new { id = commentModel.PostId });
        }
    }
}