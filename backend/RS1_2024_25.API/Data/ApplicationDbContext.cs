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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountProduct> DiscountProducts { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }


        #region METHODS
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Postavi Restrict kao DEFAULTNO ponašanje za SVE FK
            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== SVE FK relacionalnosti na Restrict ==========
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

           

            // Cart -> CartItem
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

        
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

         
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

        }
        #endregion
    }
}
