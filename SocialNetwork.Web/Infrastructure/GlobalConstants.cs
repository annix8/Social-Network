namespace SocialNetwork.Web.Infrastructure
{
    using System.Collections.Generic;

    public class GlobalConstants
    {
        public static string[] PictureFileNameExtensions => new string[]
        {
            "jpeg",
            "jpg",
            "png"
        };

        public static List<string> UserRoles => new List<string>
        {
            UserRole.User,
            UserRole.Administrator
        };

        public class UserRole
        {
            public const string User = "User";
            public const string Administrator = "Administrator";
        }
    }
}
