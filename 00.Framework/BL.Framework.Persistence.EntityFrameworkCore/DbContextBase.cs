using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Framework.Persistence.EntityFrameworkCore
{
    public class DbContextBase : DbContext
    {
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Assembly ConfigurationAssembly { get; set; }

        public DbSet<Audit> AuditLogs { get; set; }

        public DbContextBase(DbContextOptions options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbContextBase(DbContextOptions options, ILogger logger, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.ApplyConfigurationsFromAssembly(ConfigurationAssembly);
        }

        public override int SaveChanges()
        {
            try
            {
                UpdateEntities();
                AddAuditLogs();

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
                AddAuditLogs();

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
                AddAuditLogs();

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
                AddAuditLogs();

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
                AddAuditLogs();

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

        private void AddAuditLogs()
        {
            ChangeTracker.DetectChanges();

            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    EntityName = entry.Entity.GetType().Name,
                    UserId = _httpContextAccessor.HttpContext?.User?.FindFirst("sub").Value
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;

                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;

                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;

                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedProperties.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}