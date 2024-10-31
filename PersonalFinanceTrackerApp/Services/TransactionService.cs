using PersonalFinanceTrackerApp.Data;
using PersonalFinanceTrackerApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceTrackerApp.Services
{
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

        public Task AddTransactionAsync(decimal amount, string category, DateTime date, string type)
        {
            var transaction = new Transaction
            {
                Amount = amount,
                Category = category,
                Date = date,
                Type = type
            };
            return _transactionRepository.AddTransactionAsync(transaction);
        }

        public Task UpdateTransactionAsync(int id, string category, decimal amount, DateTime date, string type)
        {
            var transaction = new Transaction
            {
                Id = id,
                Category = category,
                Amount = amount,
                Date = date,
                Type = type
            };
            return _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public Task DeleteTransactionAsync(int id)
        {
            return _transactionRepository.DeleteTransactionAsync(id);
        }

        public Task<FinancialSummary> GetFinancialSummaryAsync()
        {
            return _transactionRepository.GetFinancialSummaryAsync();
        }

        public Task<List<CategorySummary>> GetCategorySummaryAsync()
        {
            return _transactionRepository.GetCategorySummaryAsync();
        }
    }
}
