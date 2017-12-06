namespace SocialNetwork.Services
{
    using DataModel;
    using DataModel.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Services.Contracts;
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using SocialNetwork.DataModel.Enums;

    public class PostService : IPostService
    {
        private readonly SocialNetworkDbContext _db;

        public PostService(SocialNetworkDbContext db)
        {
            _db = db;
        }

        public async Task<Post> ByIdAsync(int id)
        {
            return await _db.Posts
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Include(p => p.Picture)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> ByUserIdAsync(string ownerId, int page = 1, int pageSize = 10)
        {
            return await _db.Posts
                .Include(p => p.Picture)
                .Where(p => p.UserId == ownerId)
                .OrderByDescending(p => p.PublishedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> ByUserIdCountAsync(string ownerId)
        {
            return await _db.Posts
                .Where(p => p.UserId == ownerId)
                .CountAsync();
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

        public async Task<IEnumerable<Post>> FriendsPostsAsync(string userId)
        {
            var friendsIds = await _db.Friendships
                .Where(fr => fr.UserId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .Select(x => x.FriendId)
                .ToListAsync();

            var friendsUsersIds = await _db.Friendships
                .Where(fr => fr.FriendId == userId && fr.FriendshipStatus == FriendshipStatus.Accepted)
                .Select(x => x.UserId)
                .ToListAsync();

            friendsIds.AddRange(friendsUsersIds);

            if (!friendsIds.Any())
            {
                return new List<Post>();
            }

            var random = new Random();
            var posts = new List<Post>();
            for (int i = 1; i <= 10; i++)
            {
                var randomFriendId = friendsIds[random.Next(0, friendsIds.Count)];

                var friendPost = await _db.Posts
                    .Include(p => p.Picture)
                    .Include(p => p.User)
                    .OrderByDescending(p => p.PublishedOn)
                    .FirstOrDefaultAsync(p => p.UserId == randomFriendId);

                if(friendPost != null && !posts.Contains(friendPost))
                {
                    posts.Add(friendPost);
                }
            }

            return posts;
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
