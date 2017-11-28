using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Services.Contracts
{
    public interface IPostService
    {
        Task<bool> CreateAsync(string publisher, string title, string content, IFormFile picture);
        Task<bool> CreateAsync(string publisher, string title, string content);

    }
}
