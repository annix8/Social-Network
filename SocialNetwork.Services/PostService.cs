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

        /// <summary>
        /// This method loads user, comments and their authors and the picture of the post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Post> ByIdAsync(int id)
        {
            return await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Picture)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> CreateAsync(string publisher, string title, string content, IFormFile pictureFile)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == publisher);
            if (user == null)
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

        public async Task<bool> DeleteAsync(int postId)
        {
            var post = await _db.Posts.FindAsync(postId);

            if (post == null)
            {
                return false;
            }
            
            if(post.PictureId != null)
            {
                var pic = _db.Pictures.Find(post.PictureId);

                _db.Pictures.Remove(pic);
            }
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditAsync(int postId, string title, string content)
        {
            var post = await _db.Posts
                .Include(p => p.Picture)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                return false;
            }

            post.Title = title;
            post.Content = content;
            post.EditedOn = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> MakeCommentAsync(string commentContent, int postId, string commentAuthor)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == commentAuthor);

            if (user == null)
            {
                return false;
            }

            var post = await _db.Posts.FindAsync(postId);

            if (post == null)
            {
                return false;
            }

            var comment = new Comment
            {
                Content = commentContent,
                Post = post,
                User = user
            };

            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
