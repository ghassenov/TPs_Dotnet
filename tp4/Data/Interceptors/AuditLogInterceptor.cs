using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MoviesCrudApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MoviesCrudApp.Data.Interceptors
{
    public class AuditLogInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            AuditChanges(eventData.Context);
            return result;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            AuditChanges(eventData.Context);
            return new ValueTask<InterceptionResult<int>>(result);
        }

        private void AuditChanges(DbContext context)
        {
            if (context == null) return;

            var auditLogs = new List<AuditLog>();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog)
                    continue;

                var auditLog = new AuditLog();

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditLog.ChangeType = "Added";
                        auditLog.NewValues = GetNewValues(entry);
                        break;

                    case EntityState.Modified:
                        auditLog.ChangeType = "Modified";
                        auditLog.OldValues = GetOldValues(entry);
                        auditLog.NewValues = GetNewValues(entry);
                        break;

                    case EntityState.Deleted:
                        auditLog.ChangeType = "Deleted";
                        auditLog.OldValues = GetOldValues(entry);
                        break;

                    default:
                        continue;
                }

                auditLog.EntityName = entry.Entity.GetType().Name;
                auditLog.EntityId = GetEntityId(entry);
                auditLog.ChangeDate = DateTime.Now;
                auditLog.UserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                auditLogs.Add(auditLog);
            }

            // Add audit logs to the context
            foreach (var log in auditLogs)
            {
                context.Set<AuditLog>().Add(log);
            }
        }

        private string GetNewValues(EntityEntry entry)
        {
            var values = new Dictionary<string, object>();
            foreach (var property in entry.Properties)
            {
                values[property.Metadata.Name] = property.CurrentValue;
            }
            return JsonSerializer.Serialize(values);
        }

        private string GetOldValues(EntityEntry entry)
        {
            var values = new Dictionary<string, object>();
            foreach (var property in entry.Properties)
            {
                values[property.Metadata.Name] = property.OriginalValue;
            }
            return JsonSerializer.Serialize(values);
        }

        private int GetEntityId(EntityEntry entry)
        {
            var keyProperty = entry.Metadata.FindPrimaryKey().Properties[0];
            return (int)entry.Property(keyProperty.Name).CurrentValue;
        }
    }
}
