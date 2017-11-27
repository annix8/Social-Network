using SocialNetwork.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IPictureService
    {
        Task<Picture> ById(int id);
    }
}
