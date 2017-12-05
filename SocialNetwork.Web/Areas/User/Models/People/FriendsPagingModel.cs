namespace SocialNetwork.Web.Areas.User.Models.People
{
    using System.Collections.Generic;
    using DataModel.Models;

    public class FriendsPagingModel
    {
        public IEnumerable<User> Friends { get; set; }
        public string UserId { get; set; }
        public string Names { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
