#nullable enable
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Infrastructure.Extensions;

/// <summary>
/// Provides extension methods for DbContext to apply specific configurations.
/// </summary>
public static class DbContextExtensions
{
    /// <summary>
    /// Applies the recommended PRAGMA settings for a shared-cache SQLite database,
    /// including setting the Journal Mode to Delete for better network drive stability.
    /// </summary>
    public static void ApplySharedCachePragma(this DbContext context)
    {
        var connection = context.Database.GetDbConnection() as SqliteConnection;
        if (connection?.State != System.Data.ConnectionState.Open)
        {
            connection?.Open();
        }

        if (connection != null)
        {
            using (var command = connection.CreateCommand())
            {
                // This PRAGMA command is the standard way to set the journal mode.
                command.CommandText = "PRAGMA journal_mode = WAL;";
                command.ExecuteNonQuery();
            }
        }
    }
}