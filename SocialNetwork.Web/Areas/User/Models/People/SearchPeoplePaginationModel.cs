namespace SocialNetwork.Web.Areas.User.Models.People
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataModel.Models;

    public class SearchPeoplePaginationModel
    {
        public IEnumerable<User> Users { get; set; }
        public string UsernameToSearch { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PreviousPage => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;
        public int NextPage => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
