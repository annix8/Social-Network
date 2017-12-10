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
        [Required]
        public string Content { get; set; }
        public DateTime Date { get; set; }
    }
}
