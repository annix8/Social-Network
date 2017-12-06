namespace SocialNetwork.Web.Areas.User.Models.Profile
{
    using DataModel.Models;
    using System.Collections.Generic;

    public class MyProfileModel
    {
        public User User { get; set; }
        public IEnumerable<Post> Posts { get; set; }
        public int PendingRequestsCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
