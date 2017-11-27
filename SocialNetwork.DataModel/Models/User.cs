using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.DataModel.Models
{
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public ICollection<Album> Albums { get; set; } = new HashSet<Album>();
        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Friendship> FriendRequestsMade { get; set; } = new HashSet<Friendship>();
        public ICollection<Friendship> FriendRequestsAccepted { get; set; } = new HashSet<Friendship>();
        [Required]
        public bool IsPublic { get; set; }
        public Picture ProfilePicture { get; set; }
        public int? ProfilePictureId { get; set; }
    }
}
