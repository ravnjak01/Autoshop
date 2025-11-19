using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Data.Models.Modul3_Audit;
using RS1_2024_25.API.Data.Models.ShoppingCart;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;
using RS1_2024_25.API.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using Category = RS1_2024_25.API.Data.Models.ShoppingCart.Category;
using Product = RS1_2024_25.API.Data.Models.ShoppingCart.Product;

namespace RS1_2024_25.API.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

       
        public DbSet<MyAuthenticationToken> MyAuthenticationTokensAll { get; set; }
      
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogRating> BlogRatings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountProduct> DiscountProducts { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
   
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        #region METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");
      

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100); 
            });
            modelBuilder.Entity<MyAuthenticationToken>().HasNoKey();

            modelBuilder.Entity<DiscountCode>()
                        .HasIndex(x => x.Code)
                        .IsUnique();

            modelBuilder.Entity<DiscountProduct>()
                        .HasIndex(x => new { x.DiscountId, x.ProductId })
                        .IsUnique();

            modelBuilder.Entity<DiscountCategory>()
                        .HasIndex(x => new { x.DiscountId, x.CategoryId })
                        .IsUnique();
            // opcija kod nasljeđivanja
            // modelBuilder.Entity<NekaBaznaKlasa>().UseTpcMappingStrategy();
        

            modelBuilder.Entity<Category>().HasData(
       new Category { Id = 1, Name = "Electronics", Code = "ELEC" },
       new Category { Id = 2, Name = "Clothing", Code = "CLOTH" },
       new Category { Id = 3, Name = "Books", Code = "BOOKS" }
   );
        }
        #endregion
    }
}
