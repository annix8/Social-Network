﻿namespace SocialNetwork.Services.Contracts
{
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostService
    {
        Task<bool> CreateAsync(string publisher, string title, string content, IFormFile picture);

        Task<bool> CreateAsync(string publisher, string title, string content);

        Task<bool> EditAsync(int postId, string title, string content);

        Task<bool> DeleteAsync(int postId);

        Task<Post> ByIdAsync(int id);

        Task<bool> MakeCommentAsync(string commentContent, int postId, string commentAuthor);

        Task<IEnumerable<Post>> ByUserIdAsync(string ownerId, int page = 1, int pageSize = 10);

        Task<int> ByUserIdCountAsync(string ownerId);
    }
}
