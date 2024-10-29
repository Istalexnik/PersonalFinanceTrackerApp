namespace PersonalFinanceTrackerApp.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string Type { get; set; } = "Expense"; // "Income" or "Expense"
}
