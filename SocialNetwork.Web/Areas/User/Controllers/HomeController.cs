using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    [Authorize]
    public class HomeController : UserAreaController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}