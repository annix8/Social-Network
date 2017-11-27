using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.DataModel.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public Picture Picture { get; set; }
        public int? PictureId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        [Required]
        public DateTime PublishedOn { get; set; }
        public DateTime? EditedOn { get; set; }
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}
