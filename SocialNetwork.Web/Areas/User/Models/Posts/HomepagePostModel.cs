namespace SocialNetwork.Web.Areas.User.Models.Posts
{
    using DataModel.Models;
    using System.Collections.Generic;

    public class HomepagePostModel
    {
        public IEnumerable<Post> Posts { get; set; }
    }
}
