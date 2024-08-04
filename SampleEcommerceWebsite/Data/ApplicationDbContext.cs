using Microsoft.EntityFrameworkCore;
using SampleEcommerceWebsite.Models;

namespace SampleEcommerceWebsite.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Tops", DisplayOrder = 1 },
                new Category { CategoryId = 2, Name = "Bottoms", DisplayOrder = 2 },
                new Category { CategoryId = 3, Name = "Accessories", DisplayOrder = 3 }
                );
        }
    }
}
