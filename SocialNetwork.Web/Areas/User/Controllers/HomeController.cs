using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SocialNetwork.Web.Areas.User.Controllers
{
    public class HomeController : UserAreaController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}