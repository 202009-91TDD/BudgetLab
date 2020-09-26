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



            var yearMonth = startDate.ToString("yyyyMM");

            var amount = (double)budgets.Where(x=>x.YearMonth == yearMonth).Sum(x=>x.Amount);

            var daysInMonth = DateTime.DaysInMonth(startDate.Year,startDate.Month);

            return (amount / daysInMonth) * ((endDate - startDate).Days+1);
        }
    }
}