namespace SocialNetwork.Web.Areas.User.Models.Messages
{
    using SocialNetwork.DataModel.Models;
    using System.Collections.Generic;

    public class ReceivedMessagesPaginationModel
    {
        public IEnumerable<Message> Messages { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
