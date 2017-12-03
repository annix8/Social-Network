namespace SocialNetwork.Web.Areas.User.Models.Profile
{
    using DataModel.Models;
    using SocialNetwork.DataModel.Enums;

    public class VisitProfileModel
    {
        public User User { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }
        public string IssuerUsername { get; set; }
    }
}
