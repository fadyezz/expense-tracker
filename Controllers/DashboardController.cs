﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Expenses.Models;
using System.Globalization;
namespace Expenses.Controllers
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
            DateTime StartDate = DateTime.Today.AddDays(-6);
            DateTime EndDate = DateTime.Today;

            List<Transactions> SelectedTransactions = await _context.Transactions
                .Include(x => x.Category)
                .Where(y => y.Date >= StartDate && y.Date <= EndDate)
                .ToListAsync();

            //Total Income
            int TotalIncome = SelectedTransactions.Where(i => i.Category.Type == "Income").Sum(x => x.Amount);
            ViewBag.TotalIncome = TotalIncome.ToString("C0");

            //Total Expense
            int TotalExpense = SelectedTransactions.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);
            ViewBag.TotalExpense = TotalExpense.ToString("C0");

            //Total Balance
            int Balance = TotalIncome - TotalExpense;
            CultureInfo culture = new CultureInfo("en-US");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = Balance.ToString("C0");

            // Doughnut Chart - Expense By Category
            ViewBag.DoughnutChartData = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Category.CategoryId)
                .Select(k => new
                {
                    categoryTitleWithIcon = k.First().Category.Icon + " " + k.First().Category.Title,
                    amount = k.Sum(j => j.Amount),
                    formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
                })
                .OrderByDescending(l => l.amount)
                .ToList();



            /// spline chart - income vs expenses
            /// income

            List<SplineChartData> IncomeSummary= SelectedTransactions
                .Where(i => i.Category.Type == "Income")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),income= k.Sum(j => j.Amount)
                })
                .ToList();

            /// expense
            /// 
            List<SplineChartData> ExpenseSummary = SelectedTransactions
                .Where(i => i.Category.Type == "Expense")
                .GroupBy(j => j.Date)
                .Select(k => new SplineChartData()
                {
                    day = k.First().Date.ToString("dd-MMM"),
                    expense = k.Sum(j => j.Amount)
                })
                .ToList();


            //combine
            string[] last7day = Enumerable.Range(0, 7).Select(i => StartDate.AddDays(i).ToString("dd-MMM")).ToArray();

            ViewBag.SplineChartData = from day in last7day
                                      join income in IncomeSummary on day equals income.day into dayIncomeJoined
                                      from income in dayIncomeJoined.DefaultIfEmpty()
                                      join expense in ExpenseSummary on day equals expense.day into expenseJoined
                                      from expense in expenseJoined.DefaultIfEmpty()

                                      select new
                                      {
                                          day = day,
                                          income = income == null ? 0 : income.income,
                                          expense = expense == null ? 0 : expense.expense,
                                      };
            ///recent tranx
            ///

            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category).OrderByDescending(i => i.Date).Take(5).ToListAsync();


            return View();
        }
    }


    public class SplineChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
