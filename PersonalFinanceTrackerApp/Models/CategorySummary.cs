namespace PersonalFinanceTrackerApp.Models;

public class CategorySummary
{
    public string Category { get; set; } = string.Empty;
    public decimal Income { get; set; }
    public decimal Expenses { get; set; }
}
