using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public class DbContextBase : DbContext
    {
        private ILogger _logger;
        public Assembly ConfigurationAssembly { get; set; }

        public DbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbContextBase(DbContextOptions options, ILogger logger) : base(options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(ConfigurationAssembly);
        }

        public override int SaveChanges()
        {
            try
            {
                UpdateEntities();

                var result = base.SaveChanges();

                _logger.LogInformation($"Save Changes result: {result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Save Changes Error: {ex.Message}", ex);

                return -1;
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                UpdateEntities();

                var result = base.SaveChanges(acceptAllChangesOnSuccess);

                _logger.LogInformation($"Save Changes result: {result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Save Changes Error: {ex.Message}", ex);

                return -1;
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                UpdateEntities();

                var result = await base.SaveChangesAsync(cancellationToken);

                _logger.LogInformation($"Save Changes Async result: {result}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException != null ? ex.InnerException.Message : ex.Message, ex);

                return -1;
            }
        }

        public int SaveChangesTransactional()
        {
            using var transaction = Database.BeginTransaction();

            try
            {
                UpdateEntities();

                var result = SaveChanges();
                transaction.Commit();

                _logger.LogInformation($"Save Changes Transactional result: {result}");

                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                _logger.LogError(ex.Message, ex); return -1;
            }
        }

        public async Task<int> SaveChangesTransactionalAsync()
        {
            await using var transaction = await Database.BeginTransactionAsync();

            try
            {
                UpdateEntities();

                var result = await SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"Save Changes Transactional Async result: {result}");

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                _logger.LogError(ex.InnerException != null ? ex.InnerException.Message : ex.Message, ex);

                return -1;
            }
        }

        private void UpdateEntities()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is EntityCoreBase && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityCoreBase)entityEntry.Entity).CreatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    if (((EntityCoreBase)entityEntry.Entity).IsDeleted)
                    {
                        ((EntityCoreBase)entityEntry.Entity).DeletedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                    else
                    {
                        ((EntityCoreBase)entityEntry.Entity).LastUpdatedOn = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    }
                }
            }
        }
    }
}
