namespace SocialNetwork.Web.Areas.User.Models.Posts
{
    using System.ComponentModel.DataAnnotations;

    public class CommentModel
    {
        public int PostId { get; set; }
        [Required]
        public string CommentAuthor { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
