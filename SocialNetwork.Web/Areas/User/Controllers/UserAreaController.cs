namespace SocialNetwork.Web.Areas.User.Controllers
{
    using DataModel.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Infrastructure;

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

        protected (bool, List<string>) ValidateFile(IFormFile file)
        {
            var errors = new List<string>();
            var hasErrors = false;
            var fileNameTokens = file.FileName.Split('.');
            double fileMb = (double)file.Length / (1024 * 1024);

            if (file == null)
            {
                errors.Add("You must provide a file.");
                hasErrors = true;
            }

            if (fileMb > 2.5)
            {
                errors.Add("Allowed maximum size of file is 2.5mb.");
                hasErrors = true;
            }

            if (!GlobalConstants.PictureFileNameExtensions.Contains(fileNameTokens[fileNameTokens.Length - 1]))
            {
                errors.Add($"Allowed file extensions are: {string.Join(", ", GlobalConstants.PictureFileNameExtensions)}");
                hasErrors = true;
            }

            return (hasErrors, errors);
        }
    }
}