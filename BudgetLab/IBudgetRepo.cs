using System.Collections.Generic;

namespace BudgetLab
{
    public interface IBudgetRepo
    {
        List<Budget> GetAll();
    }
}