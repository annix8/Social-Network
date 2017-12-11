namespace SocialNetwork.Services.Contracts
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPictureService
    {
        Task<Picture> ByIdAsync(int id);

        Task<bool> UploadProfilePictureAsync(string username, IFormFile picture);

        Task<IEnumerable<Album>> UserAlbumsAsync(string userId);

        Task<bool> CreateAlbumAsync(string albumName, string description, string userId);
    }
}
