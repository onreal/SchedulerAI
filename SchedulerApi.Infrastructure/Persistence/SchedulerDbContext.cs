using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchedulerApi.Domain.Shared.Contracts;
using SchedulerApi.Infrastructure.Persistence.Entities;
using SchedulerApi.Infrastructure.Services;

namespace SchedulerApi.Infrastructure.Persistence
{
    public class SchedulerDbContext : DbContext
    {
        private readonly ITenantProvider _tenantProvider;

        public SchedulerDbContext(
            DbContextOptions<SchedulerDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public DbSet<UserEf> Users => Set<UserEf>();
        public DbSet<ScheduleEf> Schedules => Set<ScheduleEf>();
        public DbSet<TemplateEf> Templates => Set<TemplateEf>();
        public DbSet<UserIntegrationEf> UserIntegrations => Set<UserIntegrationEf>();
        public DbSet<RecipientEf> Recipients => Set<RecipientEf>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.ApiKey).IsRequired();
                entity.Property(e => e.Plan).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UsageCount).HasDefaultValue(0);
            });
            
            modelBuilder.Entity<ScheduleEf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.IntegrationIds);
                entity.Property(e => e.EventTime).IsRequired();
                entity.Property(e => e.NotifyBeforeMinutes).IsRequired();
                entity.Property(e => e.RecipientIds).IsRequired();
                entity.Property(e => e.IsSent).IsRequired();
                entity.Property(e => e.SentAt);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                entity.HasOne(s => s.User)
                    .WithMany() // optionally .WithMany(u => u.Schedules)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<TemplateEf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                
                entity.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<UserIntegrationEf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.TemplateId).IsRequired();
                entity.Property(e => e.Order).IsRequired();
                entity.Property(e => e.IntegrationId).IsRequired();
                entity.Property(e => e.Settings).IsRequired();
                
                entity.HasOne(i => i.User)
                    .WithMany()
                    .HasForeignKey(i => i.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(i => i.Template)
                    .WithMany()
                    .HasForeignKey(i => i.TemplateId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            
            modelBuilder.Entity<RecipientEf>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Surname).IsRequired();
                entity.Property(e => e.Phone).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                
                entity.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IUserScopedEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(SchedulerDbContext)
                        .GetMethod(nameof(SetUserFilter), BindingFlags.NonPublic | BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { modelBuilder, _tenantProvider });
                }
            }
        }

        private static void SetUserFilter<TEntity>(ModelBuilder builder, ITenantProvider provider)
            where TEntity : class, IUserScopedEntity
        {
            if (provider.GetIsProviderRequired() && provider.GetCurrentUserId() == null)
            {
                throw new UnauthorizedAccessException("Provider cannot be accessed");
            }

            if (!provider.GetIsProviderRequired() && provider.GetCurrentUserId() == null)
            {
                return;
            }

            builder.Entity<TEntity>()
                .HasQueryFilter(e => e.UserId == provider.GetCurrentUserId());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!_tenantProvider.GetIsProviderRequired() && _tenantProvider.GetCurrentUserId() == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            
            if (_tenantProvider.GetCurrentUserId() == null)
            {
                throw new UnauthorizedAccessException("Provider cannot be accessed");
            }
            
            foreach (var entry in ChangeTracker.Entries<IUserScopedEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UserId = (Guid)_tenantProvider.GetCurrentUserId()!;
                }

                if ((entry.State == EntityState.Modified || entry.State == EntityState.Deleted) &&
                    entry.Entity.UserId != _tenantProvider.GetCurrentUserId())
                {
                    throw new UnauthorizedAccessException("Access denied to another tenant's data.");
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}