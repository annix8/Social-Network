﻿namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Services.Contracts;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Areas.User.Models.Posts;
    using Web.Infrastructure;
    using Web.Infrastructure.Extensions;

    public class PostsController : UserAreaController
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;

        public PostsController(IPostService postService, IUserService userService, UserManager<User> userManager)
        {
            _postService = postService;
            _userService = userService;
            _userManager = userManager;
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

            if (post == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            var loggedUserId = _userManager.GetUserId(User);

            if (!await this.CheckFriendshipStatus(post.UserId, loggedUserId, _userService))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentModel commentModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .Where(v => v.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                
                TempData.AddErrorMessage(string.Join(", ", errors));
                return RedirectToAction(nameof(Details), new { id = commentModel.PostId });
            }

            var post = await _postService.ByIdAsync(commentModel.PostId);
            if (post == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            var loggedUserId = _userManager.GetUserId(User);

            if (!await this.CheckFriendshipStatus(post.UserId, loggedUserId, _userService))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            await _postService.MakeCommentAsync(commentModel.Comment, commentModel.PostId, commentModel.CommentAuthor);

            return RedirectToAction(nameof(Details), new { id = commentModel.PostId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _postService.CommentByIdAsync(commentId);
            if(comment == null)
            {
                return BadRequest();
            }

            if(comment.User.UserName != User.Identity.Name &&
                !User.IsInRole(GlobalConstants.UserRole.Administrator)
                && comment.Post.User.UserName != User.Identity.Name)
            {
                return BadRequest();
            }

            var result = await _postService.DeleteCommentByIdAsync(commentId);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.ByIdAsync(id);
            if (post == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            if (post.User.UserName != User.Identity.Name)
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
            if (!ModelState.IsValid)
            {
                return View(postModel);
            }

            var post = await _postService.ByIdAsync(postModel.PostId);
            if (post == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            if (post.User.UserName != User.Identity.Name)
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var postResult = await _postService.EditAsync(postModel.PostId, postModel.Title, postModel.Content);

            return RedirectToAction(nameof(ProfileController.MyProfile), "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PostModel postModel)
        {
            var post = await _postService.ByIdAsync(postModel.PostId);
            if (post == null)
            {
                return View(GlobalConstants.NotFoundView);
            }

            if (post.User.UserName != User.Identity.Name && !User.IsInRole(GlobalConstants.UserRole.Administrator))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            var deleteResult = await _postService.DeleteAsync(postModel.PostId);

            return Ok();
        }
    }
}