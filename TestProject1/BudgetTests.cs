using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace TestProject1;

/// <summary>
/// prompt: 用Nunit framework + NSubstitute產生QueryTotalAmount的測試，startDate = 2025/7/30，endDate = 2025/8/14，mock IBudgetRepo.GetAll回傳[{"202507", 140}, {"202508", 1400}]
/// </summary>
[TestFixture]
public class BudgetManagerTests
{
    [Test]
    public void QueryTotalAmount_CrossTwoMonths_ReturnsCorrectAmount()
    {
        // Arrange
        var repo = Substitute.For<IBudgetRepo>();
        repo.GetAll().Returns(new List<Budget>
        {
            new Budget { YearMonth = "202507", Amount = 310 },
            new Budget { YearMonth = "202508", Amount = 3100 }
        });

        var manager = new BudgetManager(repo);
        var startDate = new DateTime(2025, 7, 30);
        var endDate = new DateTime(2025, 8, 14);

        // Act
        var total = manager.QueryTotalAmount(startDate, endDate);

        // 2025/7/30~2025/7/31: 2天, 310/31*2
        // 2025/8/1~2025/8/14: 14天, 3100/31*14
        var expected = (310 / 31 * 2) + (3100 / 31 * 14);

        // Assert
        Assert.AreEqual(expected, total);
    }
}
/// <summary>
/// prompt: 產生BudgetManager class, 包含方法QueryTotalAmount，input有startDate和endDate，output decimal totalAmount，這個方法會使用BudgetRepo拿到一組Budget，每個Budget包含{string YearMonth, decimal Amount}，totalAmount是把startDate到endDate之間的天數的Amount加總
/// </summary>
public class BudgetManager
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetManager(IBudgetRepo budgetRepo)
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