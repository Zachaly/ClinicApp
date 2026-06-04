using ClinicApp.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClinicApp.Infrastructure.Interceptor;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if(eventData.Context is null)
        {
            return base.SavedChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData
            .Context
            .ChangeTracker
            .Entries<ISoftDelete>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (var entry in entries) 
        {
            entry.State = EntityState.Modified;
            entry.Entity.DeletedOn = DateTimeOffset.UtcNow;
        }

        return base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
