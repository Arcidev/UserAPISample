using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            builder.Entity<Phone>()
                .HasKey(x => new { x.UserId, x.Number });
        }
    }
}
