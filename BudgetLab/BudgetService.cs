using System;
using System.Linq;
using NUnit.Framework.Constraints;

namespace BudgetLab
{
    public class BudgetService
    {
        private readonly IBudgetRepo _budgetRepo;

        public BudgetService(IBudgetRepo budgetRepo)
        {
            _budgetRepo = budgetRepo;
        }

        public double Query(DateTime startDate, DateTime endDate)
        {
            if (startDate.CompareTo(endDate) > 0)
            {
                return 0;
            }

            var budgets = _budgetRepo.GetAll();

            var start_yearMonth = startDate.ToString("yyyyMM");
            var end_yearMonth = endDate.ToString("yyyyMM");
            var startBudget = budgets.FirstOrDefault(x => x.YearMonth == start_yearMonth);

            if (start_yearMonth == end_yearMonth)
            {
                if (startBudget != null)
                {
                    return GetSingleBudget(startDate, startBudget) * ((endDate - startDate).Days + 1);
                }
            }
            else
            {
                var sum = 0d;
                if( startBudget != null )
                    sum += GetSingleBudget(startDate, startBudget) * (DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1);

                foreach (var budget in budgets)
                {
                    var yearMonth = int.Parse(budget.YearMonth);
                    if (int.Parse(start_yearMonth) < yearMonth && yearMonth < int.Parse(end_yearMonth))
                        sum += budget.Amount;
                }
                var endBudget = budgets.FirstOrDefault(x => x.YearMonth == end_yearMonth);
                if (endBudget != null)
                    sum  += GetSingleBudget(endDate, endBudget) * endDate.Day;
                return sum;
            }

            return 0;
        }

        private static double GetSingleBudget(DateTime date, Budget budget)
        {
            return (double)budget.Amount / DateTime.DaysInMonth(date.Year, date.Month);
        }
    }
}