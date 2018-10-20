using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    /// <summary>
    /// Application Db context
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// User data set
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Phone data ser
        /// </summary>
        public DbSet<Phone> Phones { get; set; }

        /// <summary>
        /// Creates new instance of Db context
        /// </summary>
        /// <param name="options">Options creating db context</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <inheritdoc />
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
