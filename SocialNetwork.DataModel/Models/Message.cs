using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.DataModel.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        [Required]
        public string SenderId { get; set; }
        public User Receiver { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
