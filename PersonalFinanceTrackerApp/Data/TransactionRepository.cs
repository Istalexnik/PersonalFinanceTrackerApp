using Microsoft.Data.Sqlite;
using PersonalFinanceTrackerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalFinanceTrackerApp.Data
{
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

        public async Task<FinancialSummary> GetFinancialSummaryAsync()
        {
            var summary = new FinancialSummary();
            using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
        SELECT 
            SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END) AS TotalIncome,
            SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END) AS TotalExpenses
        FROM Transactions";

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                summary.TotalIncome = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                summary.TotalExpenses = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
            }
            return summary;
        }


        // SQL-based method to get category summary
        public async Task<List<CategorySummary>> GetCategorySummaryAsync()
        {
            var categorySummaries = new List<CategorySummary>();
            using var connection = new SqliteConnection($"Data Source={DatabaseConfig.DatabasePath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT 
                    Category,
                    SUM(CASE WHEN Type = 'Income' THEN Amount ELSE 0 END) AS Income,
                    SUM(CASE WHEN Type = 'Expense' THEN Amount ELSE 0 END) AS Expenses
                FROM Transactions
                GROUP BY Category";

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                categorySummaries.Add(new CategorySummary
                {
                    Category = reader.GetString(0),
                    Income = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                    Expenses = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2)
                });
            }
            return categorySummaries;
        }
    }
}
