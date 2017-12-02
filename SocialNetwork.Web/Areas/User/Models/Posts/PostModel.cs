using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Web.Areas.User.Models.Posts
{
    public class PostModel
    {
        public int PostId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public IFormFile Picture { get; set; }
    }
}
