using Microsoft.AspNetCore.Http;
using SocialNetwork.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IPictureService
    {
        Task<Picture> ByIdAsync(int id);
        Task<bool> UploadProfilePictureAsync(string username, IFormFile picture);
    }
}
