using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>(e =>
            {
                e.HasMany(Uri => Uri.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            modelBuilder.Entity<AppRole>(e =>
            {
                e.HasMany(Uri => Uri.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
            });


            modelBuilder.Entity<Photo>(e =>
            {
                e.HasKey(p => new { p.Id });
                e.HasOne(p => p.AppUser)
                .WithMany(b => b.Photos)
                .HasForeignKey(p => p.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
            });  

            modelBuilder.Entity<UserLike>(e =>
            {
                e.HasKey(k => new { k.SourceUserId, k.TargetUserId });
                
                e.HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);
            });              

            modelBuilder.Entity<Message>(e =>
            {
                e.HasKey(k => new { k.Id });
                
                e.HasOne(s => s.Recipient)
                .WithMany(m => m.MessageReceived)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(s => s.Sender)
                .WithMany(l => l.MessageSent)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            });              
        }   
    }
}