namespace SocialNetwork.Web.Areas.User.Models.Messages
{
    using System.ComponentModel.DataAnnotations;

    public class MessageModel
    {
        [Required]
        public string ReceiverUsername { get; set; }
        [Required]
        [MaxLength(200, ErrorMessage = "Maximum length of message is 200 symbols")]
        public string Content { get; set; }
    }
}
