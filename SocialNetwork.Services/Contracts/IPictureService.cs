namespace SocialNetwork.Services.Contracts
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<Picture> ByIdAsync(int id);

        Task<bool> UploadProfilePictureAsync(string username, IFormFile picture);
    }
}
