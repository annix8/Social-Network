namespace SocialNetwork.Web.Areas.User.Models.Profile
{
    using DataModel.Models;

    public class MyProfileModel
    {
        public User User { get; set; }
        public int PendingRequestsCount { get; set; }
    }
}
