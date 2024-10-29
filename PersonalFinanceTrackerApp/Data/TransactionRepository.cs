using Microsoft.Data.Sqlite;
using PersonalFinanceTrackerApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceTrackerApp.Data;

public class TransactionRepository
{
    public TransactionRepository()
    {
        DatabaseConfig.InitializeDatabase();
    }

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        var transactions = new List<Transaction>();
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "SELECT Id, Category, Amount, Date, Type FROM Transactions";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            transactions.Add(new Transaction
            {
                Id = reader.GetInt32(0),
                Category = reader.GetString(1),
                Amount = reader.GetDecimal(2),
                Date = DateTime.Parse(reader.GetString(3)),
                Type = reader.GetString(4)
            });
        }
        return transactions;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            INSERT INTO Transactions (Category, Amount, Date, Type) 
            VALUES ($category, $amount, $date, $type)";
        command.Parameters.AddWithValue("$category", transaction.Category);
        command.Parameters.AddWithValue("$amount", transaction.Amount);
        command.Parameters.AddWithValue("$date", transaction.Date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("$type", transaction.Type);

        await command.ExecuteNonQueryAsync();
    }

    public async Task UpdateTransactionAsync(Transaction transaction)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = @"
            UPDATE Transactions 
            SET Category = $category, Amount = $amount, Date = $date, Type = $type 
            WHERE Id = $id";
        command.Parameters.AddWithValue("$id", transaction.Id);
        command.Parameters.AddWithValue("$category", transaction.Category);
        command.Parameters.AddWithValue("$amount", transaction.Amount);
        command.Parameters.AddWithValue("$date", transaction.Date.ToString("yyyy-MM-dd"));
        command.Parameters.AddWithValue("$type", transaction.Type);

        await command.ExecuteNonQueryAsync();
    }

    public async Task DeleteTransactionAsync(int id)
    {
        using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
        await connection.OpenAsync();

        var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Transactions WHERE Id = $id";
        command.Parameters.AddWithValue("$id", id);

        await command.ExecuteNonQueryAsync();
    }
}
