using Expense_Tracker_App.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Expense_Tracker_App.Models;
using System.Globalization;

namespace Expense_Tracker_App.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            // Define date range - Last 7 Days
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            // Get transactions within the last 7 days
            List<Transaction> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(d => d.Date >= StartDate && d.Date <= EndDate)
                .ToListAsync();

            // Calculate Total Income
            int TotalIncome = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .Sum(j => j.Amount);

            // Calculate Total Expense
            int TotalExpense = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .Sum(j => j.Amount);

            // Calculate Balance
            int Balance = TotalIncome - TotalExpense;

            // Use Indian currency formatting
            CultureInfo culture = new CultureInfo("en-IN");
            culture.NumberFormat.CurrencyNegativePattern = 1;

            ViewBag.TotalIncome = TotalIncome.ToString("C0", culture);
            ViewBag.TotalExpense = TotalExpense.ToString("C0", culture);
            ViewBag.Balance = Balance.ToString("C0", culture);

            // Data for Doughnut chart (Expense by Category)
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = $"{k.First().Category.Icon} {k.First().Category.Title}",
                    amount = k.Sum(x => x.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0", culture),
                })
                .OrderByDescending(l => l.amount)
                .ToList();

            // Prepare data for the spline chart (Income vs Expense)
            // Group income and expense transactions by day

            // Income Summary
            List<SplineChartData> IncomeSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData
                {
                    day = k.First().Date.ToString("dd-MM"),
                    income = k.Sum(l => l.Amount)
                })
                .ToList();

            // Expense Summary
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData
                {
                    day = k.First().Date.ToString("dd-MM"),
                    expense = k.Sum(l => l.Amount)
                })
                .ToList();

            // Generate the last 7 days
            string[] Last7Days = Enumerable.Range(0, 7)
                .Select(i => StartDate.AddDays(i).ToString("dd-MM"))
                .ToArray();

            // Combine Income & Expense data for the spline chart
            var splineChartData = from day in Last7Days
                                  join income in IncomeSummary on day equals income.day into incomeJoined
                                  from income in incomeJoined.DefaultIfEmpty()
                                  join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                  from expense in expenseJoined.DefaultIfEmpty()
                                  select new
                                  {
                                      day,
                                      income = income?.income ?? 0,
                                      expense = expense?.expense ?? 0
                                  };

            ViewBag.SplineChartData = splineChartData.ToList();


            //recent transactions
            ViewBag.RecentTransactions = await _context.Transactions
                    .Include(i => i.Category)
                    .OrderByDescending(j => j.Date)
                    .Take(5)
                    .ToListAsync();

            return View();
        }
    }

    // Model class for SplineChartData
    public class SplineChartData
    {
        public string day { get; set; }
        public int income { get; set; }
        public int expense { get; set; }
    }
}
