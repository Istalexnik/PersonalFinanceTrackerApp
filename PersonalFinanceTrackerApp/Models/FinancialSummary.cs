namespace PersonalFinanceTrackerApp.Models;

public class FinancialSummary
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance => TotalIncome - TotalExpenses;
}
