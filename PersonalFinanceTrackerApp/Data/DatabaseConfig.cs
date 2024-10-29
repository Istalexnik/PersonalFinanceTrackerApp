using System.IO;
using Microsoft.Maui.Storage;

namespace PersonalFinanceTrackerApp.Data;

public static class DatabaseConfig
{
    public static string DatabasePath => Path.Combine(FileSystem.AppDataDirectory, "PersonalFinanceTrackerApp.db");

    public static void InitializeDatabase()
    {
        using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={DatabasePath}");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Transactions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Category TEXT NOT NULL,
                Amount REAL NOT NULL,
                Date TEXT NOT NULL,
                Type TEXT NOT NULL
            );";
        command.ExecuteNonQuery();
    }
}
