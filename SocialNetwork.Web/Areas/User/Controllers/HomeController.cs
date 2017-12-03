namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : UserAreaController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}