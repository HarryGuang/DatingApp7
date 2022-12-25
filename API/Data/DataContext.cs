using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Photo>(e =>
            {
                e.HasKey(p => new { p.Id });
                e.HasOne(p => p.AppUser)
                .WithMany(b => b.Photos)
                .HasForeignKey(p => p.AppUserId);
            });  
        }   
    }
}