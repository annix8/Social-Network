namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Posts;
    using SocialNetwork.Web.Infrastructure;
    using System.Threading.Tasks;

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

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.ByIdAsync(id);

            if(post.User.UserName != User.Identity.Name)
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var viewModel = new PostModel
            {
                PostId = post.Id,
                Content = post.Content,
                Title = post.Title
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostModel postModel)
        {
            var post = await _postService.ByIdAsync(postModel.PostId);

            if (post.User.UserName != User.Identity.Name)
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            var postResult = await _postService.EditAsync(postModel.PostId, postModel.Title, postModel.Content);

            return RedirectToAction(nameof(ProfileController.MyProfile), "Profile");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.ByIdAsync(id);

            if (post.User.UserName != User.Identity.Name && !User.IsInRole(GlobalConstants.UserRole.Administrator))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            return View(new PostModel
            {
                PostId = post.Id,
                Title = post.Title
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PostModel postModel)
        {
            var post = await _postService.ByIdAsync(postModel.PostId);

            if (post.User.UserName != User.Identity.Name && !User.IsInRole(GlobalConstants.UserRole.Administrator))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var deleteResult = await _postService.DeleteAsync(postModel.PostId);

            return RedirectToAction(nameof(ProfileController.Visit), "Profile", new { username = post.User.UserName});
        }
    }
}