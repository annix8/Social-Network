using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Web.Areas.User.Models.Posts
{
    public class CommentModel
    {
        public int PostId { get; set; }
        public string CommentAuthor { get; set; }
        public string Comment { get; set; }
    }
}
