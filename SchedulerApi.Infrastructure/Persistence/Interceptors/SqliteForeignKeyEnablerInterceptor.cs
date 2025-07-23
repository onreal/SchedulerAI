namespace SchedulerApi.Infrastructure.Persistence.Interceptors;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

public class SqliteForeignKeyEnablerInterceptor : DbConnectionInterceptor
{
    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = default)
    {
        if (connection is SqliteConnection sqliteConnection)
        {
            await using var command = sqliteConnection.CreateCommand();
            command.CommandText = $"PRAGMA foreign_keys=ON;";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }
}