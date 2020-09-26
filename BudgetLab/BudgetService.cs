using System;
using System.Linq;

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
            var endBudget = budgets.FirstOrDefault(x => x.YearMonth == end_yearMonth);
            var starRemain = DateTime.DaysInMonth(startDate.Year, startDate.Month) - startDate.Day + 1;
            var endRemain = endDate.Day;
            var startDaysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var endDaysInMonth = DateTime.DaysInMonth(endDate.Year, endDate.Month);

            if (start_yearMonth == end_yearMonth)
            {
                if (startBudget != null)
                {
                    var startMonthAmount = ((double)startBudget.Amount / startDaysInMonth) * ((endDate - startDate).Days + 1);
                    return startMonthAmount;
                }

            }
            else
            {
                if (startBudget != null && endBudget != null)
                {
                    var startMonthAmount = (startBudget.Amount / startDaysInMonth) * starRemain;
                    var endMonthAmount = (endBudget.Amount / endDaysInMonth) * endRemain;
                    return startMonthAmount + endMonthAmount;
                }
            }

            return 0;
        }
    }
}