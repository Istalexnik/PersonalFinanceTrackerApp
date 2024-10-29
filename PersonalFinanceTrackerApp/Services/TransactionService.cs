using PersonalFinanceTrackerApp.Data;
using PersonalFinanceTrackerApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceTrackerApp.Services;

public class TransactionService
{
    private readonly TransactionRepository _transactionRepository;

    public TransactionService(TransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public Task<List<Transaction>> GetAllTransactionsAsync()
    {
        return _transactionRepository.GetTransactionsAsync();
    }

    public Task AddTransactionAsync(string category, decimal amount, DateTime date, string type)
    {
        var transaction = new Transaction { Category = category, Amount = amount, Date = date, Type = type };
        return _transactionRepository.AddTransactionAsync(transaction);
    }

    public Task UpdateTransactionAsync(int id, string category, decimal amount, DateTime date, string type)
    {
        var transaction = new Transaction { Id = id, Category = category, Amount = amount, Date = date, Type = type };
        return _transactionRepository.UpdateTransactionAsync(transaction);
    }

    public Task DeleteTransactionAsync(int id)
    {
        return _transactionRepository.DeleteTransactionAsync(id);
    }

    // Method to get financial summary
    public async Task<FinancialSummary> GetFinancialSummaryAsync()
    {
        var transactions = await _transactionRepository.GetTransactionsAsync();
        return new FinancialSummary
        {
            TotalIncome = transactions.Where(t => t.Type == "Income").Sum(t => t.Amount),
            TotalExpenses = transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount)
        };
    }

    // Method to get category summary
    public async Task<List<CategorySummary>> GetCategorySummaryAsync()
    {
        var transactions = await _transactionRepository.GetTransactionsAsync();

        return transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategorySummary
            {
                Category = g.Key,
                Income = g.Where(t => t.Type == "Income").Sum(t => t.Amount),
                Expenses = g.Where(t => t.Type == "Expense").Sum(t => t.Amount)
            })
            .ToList();
    }
}
