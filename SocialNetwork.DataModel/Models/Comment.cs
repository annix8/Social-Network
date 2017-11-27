using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.DataModel.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
    }
}
