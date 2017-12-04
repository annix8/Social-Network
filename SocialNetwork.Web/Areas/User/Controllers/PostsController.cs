﻿namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Areas.User.Models.Posts;
    using SocialNetwork.Web.Infrastructure;
    using System.Threading.Tasks;
    using DataModel.Models;
    using System;
    using SocialNetwork.DataModel.Enums;

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
            var loggedUserId = _userManager.GetUserId(User);

            if (!await CheckFriendshipStatus(post.UserId, loggedUserId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentModel commentModel)
        {
            var post = await _postService.ByIdAsync(commentModel.PostId);
            var loggedUserId = _userManager.GetUserId(User);

            if (!await CheckFriendshipStatus(post.UserId, loggedUserId))
            {
                return View(GlobalConstants.AccessDeniedView);
            }

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

            return RedirectToAction(nameof(ProfileController.Visit), "Profile", new { username = post.User.UserName });
        }

        private async Task<bool> CheckFriendshipStatus(string userId, string loggedUserId)
        {
            if (User.IsInRole(GlobalConstants.UserRole.Administrator))
            {
                return true;
            }

            var (status, _) = await _userService.CheckFriendshipStatusAsync(userId, loggedUserId);

            return status == FriendshipStatus.Accepted ? true : false;
        }
    }
}