namespace SocialNetwork.Web.Areas.User.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SocialNetwork.DataModel.Enums;
    using SocialNetwork.Services.Contracts;
    using SocialNetwork.Web.Infrastructure;
    using System.Threading.Tasks;

    [Area("User")]
    [Authorize]
    public abstract class UserAreaController : Controller
    {
        protected async Task<bool> CheckFriendshipStatus(string userId, string loggedUserId)
        {
            var userService = (IUserService)this.HttpContext.RequestServices.GetService(typeof(IUserService)); 

            if (User.IsInRole(GlobalConstants.UserRole.Administrator) || userId == loggedUserId)
            {
                return true;
            }
            
            var (status, _) = await userService.CheckFriendshipStatusAsync(userId, loggedUserId);

            return status == FriendshipStatus.Accepted ? true : false;
        }
    }
}