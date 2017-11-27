using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SocialNetwork.DataModel.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<Picture> Pictures { get; set; } = new HashSet<Picture>();
    }
}
