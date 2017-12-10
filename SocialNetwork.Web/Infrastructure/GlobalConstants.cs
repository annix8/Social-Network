namespace SocialNetwork.Web.Infrastructure
{
    using System.Collections.Generic;

    public class GlobalConstants
    {
        public class UserRole
        {
            public const string User = "User";
            public const string Administrator = "Administrator";
        }

        public static List<string> UserRoles => new List<string>
        {
            UserRole.User,
            UserRole.Administrator
        };

        public static string[] PictureFileNameExtensions => new string[]
        {
            "jpeg",
            "jpg",
            "png",
            "gif",
            "bmp"
        };
        
        public static string AccessDeniedView => "/Views/Account/AccessDenied.cshtml";
        public static string NotFoundView => "/Views/Shared/NotFound.cshtml";

        public static string SuccessMessageKey => "SuccessMessage";
    }
}
