namespace SocialNetwork.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.Web.Infrastructure;

    [Authorize(Roles = GlobalConstants.UserRole.Administrator)]
    [Area("Admin")]
    public class ProfileController : Controller
    {
        public IActionResult DeleteAccount()
        {
            return View();
        }
    }
}