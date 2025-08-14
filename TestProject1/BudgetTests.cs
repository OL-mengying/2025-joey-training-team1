namespace TestProject1;

/// <summary>
/// prompt: 產生BudgetManager class, 包含方法QueryTotalAmount，input有startDate和endDate，output decimal totalAmount，這個方法會使用BudgetRepo拿到一組Budget，每個Budget包含{string YearMonth, decimal Amount}，totalAmount是把startDate到endDate之間的天數的Amount加總
/// </summary>
public class BudgetManager
{
    private readonly BudgetRepo _budgetRepo;

    public BudgetManager(BudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal QueryTotalAmount(DateTime startDate, DateTime endDate)
    {
        var budgets = _budgetRepo.GetAll();
        decimal totalAmount = 0;

        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var yearMonth = date.ToString("yyyyMM");
            var budget = budgets.FirstOrDefault(b => b.YearMonth == yearMonth);
            if (budget != null)
            {
                var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                var dailyAmount = budget.Amount / daysInMonth;
                totalAmount += dailyAmount;
            }
        }

        return totalAmount;
    }
}
public interface IBudgetRepo
{
    List<Budget> GetAll();
}

public class BudgetRepo : IBudgetRepo
{
    public List<Budget> GetAll()
    {
        // 這裡應該從資料庫或其他來源取得 Budget 資料
        return new List<Budget>();
    }
}

public class Budget
{
    public string YearMonth { get; set; }
    public decimal Amount { get; set; }
}