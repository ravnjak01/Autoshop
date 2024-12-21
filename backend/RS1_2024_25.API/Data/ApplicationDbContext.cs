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
        public int? _CurrentTenantId = null;

        public int CurrentTenantIdThrowIfFail
        {
            get
            {
                var result = CurrentTenantId;
                if (result == null || result == 0)
                {
                    throw new UnauthorizedAccessException();
                }

                return result.Value;
            }
        }
        public int? CurrentTenantId
        {
            get
            {
                if (_CurrentTenantId == null)
                {
                    var authService = serviceProvider.GetRequiredService<MyAuthService>();
                    MyAuthInfo myAuthInfo = authService.GetAuthInfoFromRequest();
                    _CurrentTenantId = myAuthInfo.TenantId;
                }
                return _CurrentTenantId;
            }
            set
            {
                _CurrentTenantId = value;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            // opcija kod nasljeđivanja
            // modelBuilder.Entity<NekaBaznaKlasa>().UseTpcMappingStrategy();

            // Iteracija kroz sve entitete u modelu
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                // Provjera da li postoji [Table("...")] atribut
                var tableAttribute = clrType.GetCustomAttributes(typeof(TableAttribute), inherit: false)
                                            .FirstOrDefault() as TableAttribute;

                if (tableAttribute == null)
                {
                    // Ako nema TableAttribute, automatski pluralizuj naziv tabele
                    var tableName = clrType.Name.Pluralize();
                    modelBuilder.Entity(clrType).ToTable(tableName);
                }
                else
                {
                    // Ako postoji TableAttribute, koristi navedeni naziv tabele
                    modelBuilder.Entity(clrType).ToTable(tableAttribute.Name);
                }
            }
        }
     
        public override int SaveChanges()
        {
            AddTenantIdToNewEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTenantIdToNewEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddTenantIdToNewEntities()
        {
            // Iteracija kroz sve promjene u DbContext
            foreach (var entry in ChangeTracker.Entries()
                         .Where(e => e.State == EntityState.Added && e.Entity is TenantSpecificTable))
            {
                // Postavljanje TenantId za nove entitete
                var entity = (TenantSpecificTable)entry.Entity;
                entity.TenantId = CurrentTenantIdThrowIfFail;
            }
        }
        #endregion
    }
}
