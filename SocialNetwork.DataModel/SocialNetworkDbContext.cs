using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.DataModel.Models;

namespace SocialNetwork.DataModel
{
    public class SocialNetworkDbContext : IdentityDbContext<User>
    {
        public SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Friendship>()
                .HasKey(k => new { k.UserId, k.FriendId });

            builder.Entity<Friendship>()
                .HasOne(u => u.User)
                .WithMany(f => f.FriendRequestsMade)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friendship>()
                .HasOne(u => u.Friend)
                .WithMany(f => f.FriendRequestsAccepted)
                .HasForeignKey(u => u.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
