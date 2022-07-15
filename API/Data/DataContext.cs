using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<Message> Messages { get; set; }

        //public DbSet<TravelRecord> TravelRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //follow function relationship linking m2m
            builder.Entity<UserFollow>()
                .HasKey(u => new { u.FollowerId, u.LeaderId });
            builder.Entity<UserFollow>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.FollowingOther)
                .HasForeignKey(s => s.FollowerId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UserFollow>()
                .HasOne(u => u.Leader)
                .WithMany(u => u.FollowedByOther)
                .HasForeignKey(s => s.LeaderId)
                .OnDelete(DeleteBehavior.NoAction);

            //messagefunction relationship linking m2m
            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}

//https://generated.photos/
