using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data.Models.Modul1_Auth;
using RS1_2024_25.API.Helper;
using RS1_2024_25.API.Helper.BaseClasses;
using RS1_2024_25.API.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace RS1_2024_25.API.Data
{
    public class ApplicationDbContext(DbContextOptions options, IServiceProvider serviceProvider) : DbContext(options)
    {
        public DbSet<MyAppUser> MyAppUsersAll { get; set; }
        public DbSet<MyAuthenticationToken> MyAuthenticationTokensAll { get; set; }

        #region METHODS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
