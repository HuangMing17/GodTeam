using GodTeam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GodTeam.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Many-to-Many relationship
            modelBuilder.Entity<PostCategory>()
                .HasKey(pc => new { pc.PostId, pc.CategoryId });

            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostId);

            modelBuilder.Entity<PostCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryId);

            // Seed default categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Tin tức", Description = "Tin tức chung" },
                new Category { Id = 2, Name = "Công nghệ", Description = "Tin tức công nghệ" },
                new Category { Id = 3, Name = "Thể thao", Description = "Tin tức thể thao" },
                new Category { Id = 4, Name = "Giải trí", Description = "Tin tức giải trí" },
                new Category { Id = 5, Name = "Khoa học", Description = "Tin tức khoa học" }
            );
        }
    }
}
