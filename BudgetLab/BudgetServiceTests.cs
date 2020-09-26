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
            GivenBudget(new List<Budget>()
            {
                new Budget() {YearMonth = "202001", Amount = 100}
            });

            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 31);
            AmountShouldBe(startDate, endDate, 100);
        }

        private void GivenBudget(List<Budget> budget)
        {
            _budgetRepo.GetAll().ReturnsForAnyArgs(budget);
        }

        [Test]
        public void no_budget()
        {
            GivenBudget(new List<Budget>());

            var startDate = new DateTime(2020, 2, 1);
            var endDate = new DateTime(2020, 2, 10);
            AmountShouldBe(startDate, endDate, 0);

        }

        [Test]
        public void single_date()
        {
            GivenBudget(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202001",Amount = 310
                }
            });

            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 1, 1);
            AmountShouldBe(startDate, endDate, 10);

        }

        [Test]
        public void cross_month_one_month()
        {
            GivenBudget(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202001",Amount = 310
                },
                new Budget()
                {
                    YearMonth = "202002",Amount = 290
                }
            });


            var startDate = new DateTime(2020, 1, 31);
            var endDate = new DateTime(2020, 2, 29);
            AmountShouldBe(startDate, endDate, 300);
        }

        [Test]
        public void cross_month_one_month_not_exist()
        {
            GivenBudget(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202001",Amount = 310
                }
            });


            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 2, 2);
            AmountShouldBe(startDate, endDate, 310);
        }

        [Test]
        public void cross_month_one_month_no_data()
        {
            GivenBudget(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202001",Amount = 310
                },
                new Budget()
                {
                    YearMonth = "202002",Amount = 0
                }
            });


            var startDate = new DateTime(2020, 1, 1);
            var endDate = new DateTime(2020, 2, 2);
            AmountShouldBe(startDate, endDate, 310);
        }

        [Test]
        public void cross_multi_month()
        {
            GivenBudget(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = "202001",Amount = 310
                },
                new Budget()
                {
                    YearMonth = "202002",Amount = 290
                },
                new Budget()
                {
                    YearMonth = "202003",Amount = 3100
                }
            });


            var startDate = new DateTime(2020, 1, 31);
            var endDate = new DateTime(2020, 3, 2);
            AmountShouldBe(startDate, endDate, 500);
        }


        private void  AmountShouldBe(DateTime startDate, DateTime endDate, double expected)
        {
            var amount = _budgetService.Query(startDate, endDate);

            Assert.AreEqual(expected, amount);
        }
    }
}