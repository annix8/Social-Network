using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Web.Infrastructure
{
    public class GlobalConstants
    {
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
