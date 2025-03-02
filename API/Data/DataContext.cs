using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// datacontext-irepository-repository(addserivce)-controller
namespace API.Data
{
    public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, 
        AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>(options)
    {
        public DbSet<UserLike> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(x => x.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(x => x.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(x => x.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();
           


            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserUId });

            builder.Entity<UserLike>()
              .HasOne(s => s.SourceUser)
              .WithMany(l => l.LikedUsers)
              .HasForeignKey(l => l.SourceUserId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
             .HasOne(s => s.TargetUser)
             .WithMany(l => l.LikedByUsers)
             .HasForeignKey(l => l.TargetUserUId)
             .OnDelete(DeleteBehavior.NoAction);
            // sql server , using can not use cascade-cascade, must be cascade - noaction 
            // but posgress allow two cascade

            //using restric when user is deleted , the messages still there, because
            // the messages have other deletion way
            builder.Entity<Message>()
              .HasOne(s => s.Sender)
              .WithMany(l => l.MessagesSent)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
             .HasOne(s => s.Recipient)
             .WithMany(l => l.MessagesReceived)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
