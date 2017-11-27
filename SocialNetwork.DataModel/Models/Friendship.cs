using SocialNetwork.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DataModel.Models
{
    public class Friendship
    {
        public User User { get; set; }
        public string UserId { get; set; }

        public User Friend { get; set; }
        public string FriendId { get; set; }

        public string FriendshipIssuerId { get; set; }

        public FriendshipStatus FriendshipStatus { get; set; }
    }
}
