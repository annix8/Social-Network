﻿using SocialNetwork.DataModel;
using SocialNetwork.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using SocialNetwork.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialNetwork.Services
{
    public class PostService : IPostService
    {
        private readonly SocialNetworkDbContext _db;

        public PostService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(string publisher, string title, string content, IFormFile pictureFile)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == publisher);
            if(user == null)
            {
                return false;
            }

            using (var stream = new MemoryStream())
            {
                await pictureFile.CopyToAsync(stream);
                var pictureData = stream.ToArray();

                var picture = new Picture
                {
                    ImageData = pictureData
                };

                var post = new Post
                {
                    Title = title,
                    Content = content,
                    Picture = picture,
                    PublishedOn = DateTime.UtcNow,
                    User = user
                };

                await _db.Posts.AddAsync(post);
                await _db.SaveChangesAsync();

                return true;
            }
        }

        public async Task<bool> CreateAsync(string publisher, string title, string content)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == publisher);
            if (user == null)
            {
                return false;
            }

            var post = new Post
            {
                Title = title,
                Content = content,
                PublishedOn = DateTime.UtcNow,
                User = user
            };

            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
