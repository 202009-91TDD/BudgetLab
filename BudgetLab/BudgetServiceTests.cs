using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace BudgetLab
{
    [TestFixture]
    public class BudgetServiceTests
    {
        private IBudgetRepo _budgetRepo;
        private BudgetService _budgetService;

        [SetUp]
        public void SetUp()
        {
            _budgetRepo = Substitute.For<IBudgetRepo>();

            _budgetService = new BudgetService(_budgetRepo);
        }

        [Test]
        public void invalid_date()
        {
            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2019, 1, 31);
            AmountShouldBe(startDate, endDate, 0);
        }

        [Test]
        public void query_whole_month()
        {
            _budgetRepo.GetAll().ReturnsForAnyArgs(new List<Budget>()
            {
                new Budget() {YearMonth = "202001", Amount = 100}
            });

            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 31);
            AmountShouldBe(startDate, endDate, 100);
        }

        private void AmountShouldBe(DateTime startDate, DateTime endDate, double expected)
        {
            var amount = _budgetService.Query(startDate, endDate);

            Assert.AreEqual(expected, amount);
        }
    }
}