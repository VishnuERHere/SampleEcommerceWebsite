using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SampleEcommerceWebsite.Models;

namespace SampleEcommerceWebsite.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //Adding New Model step 2 + migrations
        public DbSet<Company> Companies { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //included with IdentityDbContext
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Soccer", DisplayOrder = 1 },
                new Category { CategoryId = 2, Name = "Basketball", DisplayOrder = 2 },
                new Category { CategoryId = 3, Name = "Baseball", DisplayOrder = 3 },
                new Category { CategoryId = 4, Name = "Winter Sports", DisplayOrder = 4 },
                new Category { CategoryId = 5, Name = "Racquet Sports", DisplayOrder = 5 }
                );

            //Adding New Model step 8 - Seeding Values
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, 
                    Name = "Sports Retailers Ltd", 
                    StreetAddress="11 North York Road",
                    City="Toronto",
                    State="Ontario",
                    PhoneNumber="7894561230",
                    PostalCode="L1L 1L1"
                },
                new Company
                {
                    Id = 2,
                    Name = "ABC Sports",
                    StreetAddress = "21 North York Road",
                    City = "Toronto",
                    State = "Ontario",
                    PhoneNumber = "7894561231",
                    PostalCode = "L1L 1L1"
                }, new Company
                {
                    Id = 3,
                    Name = "A2Z Sports",
                    StreetAddress = "31 North York Road",
                    City = "Toronto",
                    State = "Ontario",
                    PhoneNumber = "7894561232",
                    PostalCode = "L1L 1L1"
                }
            );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Adidas Soccer Ball",
                    Brand = "Adidas",
                    Description = "Adidas Soccer Ball Size 5. ",
                    ListPrice = 30,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    CategoryId = 1,
                    ImageUrl=""
                },
                new Product
                {
                    Id = 2,
                    Name = "Nike Basketball",
                    Brand = "Nike",
                    Description = "Nike BasketBall Size 5. ",
                    ListPrice = 40,
                    Price = 40,
                    Price50 = 35,
                    Price100 = 20,
                    CategoryId = 2,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Name = "Yonex ArcSaber 11 Pro",
                    Brand = "Puma",
                    Description = "Yonex ArcSaber 11 Pro. ",
                    ListPrice = 50,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 40,
                    CategoryId = 5,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Name = "Yonex Mavis 350",
                    Brand = "Yonex",
                    Description = "Yonex Mavis 350. ",
                    ListPrice = 50,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 40,
                    CategoryId = 5,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Name = "Wilson Tennis Racquet",
                    Brand = "Wilson",
                    Description = "Wilson Tennis Racquet.",
                    ListPrice = 50,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 40,
                    CategoryId = 5,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Name = "Head Tennis Racquet",
                    Brand = "Head",
                    Description = "Head Tennis Racquet. ",
                    ListPrice = 35,
                    Price = 35,
                    Price50 = 30,
                    Price100 = 25,
                    CategoryId = 5,
                    ImageUrl = ""
                }
                );
        }
    }
}
