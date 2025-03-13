using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Data.Models.Modul2_Basic;
using RS1_2024_25.API.Data.Models.Modul3_Audit;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;
using RS1_2024_25.API.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace RS1_2024_25.API.Data
{
    public class ApplicationDbContext(DbContextOptions options, IServiceProvider serviceProvider) : DbContext(options)
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<MyAppUser> MyAppUsersAll { get; set; }
        public DbSet<MyAuthenticationToken> MyAuthenticationTokensAll { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<BlogComment> BlogComments { get; set; }
        public DbSet<BlogRating> BlogRatings { get; set; }


        #region METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>().ToTable("AuditLogs");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
            // opcija kod nasljeđivanja
            // modelBuilder.Entity<NekaBaznaKlasa>().UseTpcMappingStrategy();
        }
        #endregion
    }
}
