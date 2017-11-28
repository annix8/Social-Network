using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SocialNetwork.Web.Areas.User.Models;
using Microsoft.AspNetCore.Http;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    [Authorize]
    public class PostsController : UserAreaController
    {
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostModel postModel)
        {
            return RedirectToAction(nameof(ProfileController.MyProfile),nameof(ProfileController));
        }
    }
}