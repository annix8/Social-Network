namespace SocialNetwork.Web.Areas.User.Models.Profile
{
    using DataModel.Models;
    using SocialNetwork.DataModel.Enums;
    using System.Collections.Generic;

    public class VisitProfileModel
    {
        public User User { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public FriendshipStatus FriendshipStatus { get; set; }
        public string IssuerUsername { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
